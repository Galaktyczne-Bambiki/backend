using FluentValidation;
using Microsoft.Extensions.Options;

namespace Garager.App.Base.Options;

public abstract class FluentOptionsValidator<T> : AbstractValidator<T>, IValidateOptions<T> where T : class
{
    private readonly bool _skipOnNull;
    protected FluentOptionsValidator() : this(false)
    {

    }
    protected FluentOptionsValidator(bool skipOnNull)
    {
        _skipOnNull = skipOnNull;
    }

    protected abstract void Rules(string? name, AbstractValidator<T> builder);
    public ValidateOptionsResult Validate(string? name, T? options)
    {
        if(options is null && _skipOnNull)
            return ValidateOptionsResult.Skip;

        if (options is null)
            throw new FluentOptionsValidatorNullInstance();

        Rules(name, this);
        var results = this.Validate(options);

        if(results is null || results.IsValid)
            return ValidateOptionsResult.Success;

        var errorMessages = results.Errors
            .Select(e => e.ToString())
            .ToArray();

        return ValidateOptionsResult.Fail(errorMessages);
    }
}