using FluentValidation;
using Garager.App.Base.Options;

namespace BambikiBackend.Api.Options;

public record FirmsServiceOptions(Uri Url, string ApiKey);

public class FirmsServiceOptionsValidation : FluentOptionsValidator<FirmsServiceOptions>
{
    protected override void Rules(string? name, AbstractValidator<FirmsServiceOptions> builder)
    {
        builder.RuleFor(o => o.ApiKey).NotEmpty().WithMessage("Missing ApiKey");
        builder.RuleFor(o => o.Url).NotNull().WithMessage("Missing service url");
        builder.RuleFor(o => o.Url).Must(o => !string.IsNullOrWhiteSpace(o.Host)).WithMessage("Url is empty");
    }
}