using FluentValidation;
using AccountHub.Api.Models;

namespace AccountHub.Api.Validators;

public class BaseRequestValidator<T> : AbstractValidator<T> where T : BaseRequest
{
    protected BaseRequestValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty()
            .WithMessage("RequestId is required")
            .MaximumLength(36)
            .WithMessage("RequestId must not exceed 36 characters");

        RuleFor(x => x.Timestamp)
            .NotEmpty()
            .WithMessage("Timestamp is required")
            .Must(x => x <= DateTime.UtcNow)
            .WithMessage("Timestamp cannot be in the future");
    }
} 