using FluentValidation;

using ManagementSystem.Modules.Events.Application.DTOs;

namespace ManagementSystem.Modules.Events.Application.Validators;

public class CreateFamilyEventDtoValidator : AbstractValidator<CreateFamilyEventDto>
{
    public CreateFamilyEventDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề sự kiện là bắt buộc").MaximumLength(300);
        RuleFor(x => x.EventType).MaximumLength(50);
        RuleFor(x => x.Location).MaximumLength(300);
        RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ");
        RuleFor(x => x.StartAt).Must(d => d > DateTime.MinValue).WithMessage("Thời gian bắt đầu là bắt buộc");
        RuleFor(x => x.EndAt).GreaterThanOrEqualTo(x => x.StartAt).When(x => x.EndAt.HasValue)
            .WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");
    }
}
