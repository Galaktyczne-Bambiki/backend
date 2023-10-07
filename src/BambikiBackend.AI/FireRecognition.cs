using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace BambikiBackend.AI;

public class FireRecognition : IDisposable
{
    private readonly InferenceSession _inferenceSession;
    private readonly ILogger<FireRecognition> _logger;
    public FireRecognition(ILogger<FireRecognition> logger)
    {
        _logger = logger;
        _inferenceSession = new InferenceSession(GetONNXModel());
    }

    public byte[] GetONNXModel()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "BambikiBackend.AI.ONNX.fire_recognition.onnx";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new MemoryStream();
        stream.CopyTo(reader);

        return reader.ToArray();
    }

    public async Task<bool> HasFireOnImage(Stream stream)
    {
        var image = await Image.LoadAsync<Rgba32>(stream);
        image.Mutate(x => x.Resize(224, 224));

        DenseTensor<float> input = new(new[] { 1, 3, image.Height, image.Width });
        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                var pixelSpan = accessor.GetRowSpan(y);
                for (int x = 0; x < accessor.Width; x++)
                {
                    input[0, 0, y, x] = pixelSpan[x].B;
                    input[0, 1, y, x] = pixelSpan[x].G;
                    input[0, 2, y, x] = pixelSpan[x].R;
                }
            }
        });

        var height = image.Height;
        var width = image.Width;

        var inputs = new Dictionary<string, OrtValue>()
        {
            ["input"] = OrtValue.CreateTensorValueFromMemory(OrtMemoryInfo.DefaultInstance,
                input.Buffer, new long[] { 1, 3, height, width })
        };
        var options = new RunOptions()
        {
            LogSeverityLevel = OrtLoggingLevel.ORT_LOGGING_LEVEL_FATAL
        };

        using var outputs = _inferenceSession.Run(options, inputs, _inferenceSession.OutputNames);

        var result = outputs[0].GetTensorDataAsSpan<float>().ToArray();

        if (Math.Abs(result[0]) > Math.Abs(result[1]))
            return false;

        if (Math.Abs(result[1]) > Math.Abs(result[0]))
            return true;

        throw new Exception("Undetected result.");
    }
    public void Dispose()
    {
        _inferenceSession.Dispose();
    }
}