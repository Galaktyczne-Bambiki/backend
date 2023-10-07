using BambikiBackend.AI;
using BambikiBackend.Api.Utils;
using FluentValidation;

namespace BambikiBackend.Api.Models.FireReports;

public class FireReportRequestModel
{
    public IFormFile File { get; set; }
    public FireReportDetailsModel Details { get; set; }
}

// ReSharper disable once UnusedType.Global
public class FireReportRequestModelValidations : AbstractValidator<FireReportRequestModel>
{
    private readonly FireRecognition _fireRecognition;
    public FireReportRequestModelValidations(FireRecognition fireRecognition)
    {
        _fireRecognition = fireRecognition;

        RuleFor(o => o.File)
            .NotNull()
            .WithMessage("Image is missing");

        RuleFor(o => o.File)
            .MustAsync(async (o, ct) => await _fireRecognition.HasFireOnImage(o.OpenReadStream()))
            .WithMessage("No fire detected on image");


        RuleFor(o => o.File)
            .MustAsync(async (o, ct) =>  await HasGpsCoordinates(o.OpenReadStream(), ct))
            .When(o => o.Details.Latitude is null || o.Details.Longitude is null)
            .WithMessage("Missing GPS Cords");
    }

    internal async Task<bool> HasGpsCoordinates(Stream imageStream, CancellationToken cancellationToken)
    {
        var image = await Image.LoadAsync(imageStream, cancellationToken);

        var cords = await ExifExtensions.ExtractCoordinates(image);

        return cords.Lat is not null && cords.Lng is not null;
    }
}