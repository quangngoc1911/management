using FluentValidation;

using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Application.Validators;

public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
{
    public CreateNotificationDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Người nhận là bắt buộc");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề là bắt buộc").MaximumLength(300);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Channel).NotEmpty().MaximumLength(50);
    }
}
