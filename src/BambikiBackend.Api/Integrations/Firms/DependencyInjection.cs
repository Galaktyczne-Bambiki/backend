using BambikiBackend.Api.Options;
using Microsoft.Extensions.Options;
using Refit;

namespace BambikiBackend.Api.Integrations.Firms;

public static class DependencyInjection
{
    public static IServiceCollection AddFirmsRestClient(this IServiceCollection services)
    {
        services.AddTransient<ApiKeyDelegatingHandler>();
        services.AddRefitClient<IAreaRestClient>()
            .ConfigureHttpClient((provider, client) =>
                client.BaseAddress = provider.GetRequiredService<IOptions<FirmsServiceOptions>>().Value.Url)
            .AddHttpMessageHandler<ApiKeyDelegatingHandler>();

        return services;
    }
}