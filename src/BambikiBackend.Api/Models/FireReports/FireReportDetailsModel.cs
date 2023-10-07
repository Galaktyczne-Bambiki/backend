using FluentValidation;

namespace BambikiBackend.Api.Models.FireReports;

public class FireReportDetailsModel
{
    public string? Description { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

// ReSharper disable once UnusedType.Global
public class FireReportDetailsModelValidations : AbstractValidator<FireReportDetailsModel>
{
    public FireReportDetailsModelValidations()
    {
        RuleFor(o => o.Description)
            .Must(o => o.Length < 500)
            .When(o => !string.IsNullOrWhiteSpace(o.Description))
            .WithMessage("Message too long");
    }
}