using FluentValidation;

using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Modules.Utility.Application.Validators;

public class CreateReminderDtoValidator : AbstractValidator<CreateReminderDto>
{
    public CreateReminderDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề nhắc nhở là bắt buộc").MaximumLength(300);
        RuleFor(x => x.EntityType).MaximumLength(50);
        RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ");
        RuleFor(x => x.RemindAt).Must(d => d > DateTime.MinValue).WithMessage("Thời điểm nhắc là bắt buộc");
    }
}
