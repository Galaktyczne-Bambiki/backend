using BambikiBackend.Api.Options;
using Microsoft.Extensions.Options;

namespace BambikiBackend.Api.Integrations.Firms;

public class ApiKeyDelegatingHandler : DelegatingHandler
{
    private readonly IOptions<FirmsServiceOptions> _firmsOptions;

    public ApiKeyDelegatingHandler(IOptions<FirmsServiceOptions> firmsOptions)
    {
        _firmsOptions = firmsOptions;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.RequestUri = new Uri(request.RequestUri.ToString().Replace("[[API_KEY]]", _firmsOptions.Value.ApiKey));

        return base.SendAsync(request, cancellationToken);
    }
}