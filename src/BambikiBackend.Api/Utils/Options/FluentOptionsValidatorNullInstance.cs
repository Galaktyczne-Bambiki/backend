namespace Garager.App.Base;

public class FluentOptionsValidatorNullInstance : Exception
{
    public FluentOptionsValidatorNullInstance() : base("The instance to validate is null.")
    {

    }
    public FluentOptionsValidatorNullInstance(string? message) : base(message)
    {

    }
}