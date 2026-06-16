using FluentValidation;

using ManagementSystem.Modules.Events.Application.DTOs;

namespace ManagementSystem.Modules.Events.Application.Validators;

public class CreateEventMediaDtoValidator : AbstractValidator<CreateEventMediaDto>
{
    public CreateEventMediaDtoValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("Sự kiện là bắt buộc");
        RuleFor(x => x.FileId).NotEmpty().WithMessage("Tệp đính kèm là bắt buộc");
        RuleFor(x => x.Caption).MaximumLength(500);
    }
}
