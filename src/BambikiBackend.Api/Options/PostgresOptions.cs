using FluentValidation;
using Garager.App.Base.Options;
using Npgsql;

namespace BambikiBackend.Api.Options;

public class PostgresOptions
{
    public string ConnectionString { get; init; }

}
public class PostgresOptionsValidations : FluentOptionsValidator<PostgresOptions>
{
    protected override void Rules(string? name, AbstractValidator<PostgresOptions> builder)
    {
        builder.RuleFor(o => o.ConnectionString)
            .Must(o => !string.IsNullOrWhiteSpace(o))
            .WithMessage("Missing Postgres connection string");
    }
}