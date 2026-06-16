using FluentValidation;

using ManagementSystem.Modules.Events.Application.DTOs;

namespace ManagementSystem.Modules.Events.Application.Validators;

public class UpdateEventMediaDtoValidator : AbstractValidator<UpdateEventMediaDto>
{
    public UpdateEventMediaDtoValidator()
    {
        RuleFor(x => x.Caption).MaximumLength(500);
    }
}
