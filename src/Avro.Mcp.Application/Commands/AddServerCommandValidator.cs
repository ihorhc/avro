namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Validator for AddServerCommand
/// </summary>
public class AddServerCommandValidator : AbstractValidator<AddServerCommand>
{
    public AddServerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Server name is required")
            .MaximumLength(100).WithMessage("Server name must not exceed 100 characters");

        RuleFor(x => x.Command)
            .NotEmpty().WithMessage("Server command is required");

        RuleFor(x => x.TimeoutSeconds)
            .GreaterThan(0).WithMessage("Timeout must be greater than zero");
    }
}
