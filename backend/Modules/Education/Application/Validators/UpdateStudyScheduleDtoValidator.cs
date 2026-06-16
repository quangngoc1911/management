using FluentValidation;

using ManagementSystem.Modules.Education.Application.DTOs;

namespace ManagementSystem.Modules.Education.Application.Validators;

public class UpdateStudyScheduleDtoValidator : AbstractValidator<UpdateStudyScheduleDto>
{
    public UpdateStudyScheduleDtoValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("Thành viên là bắt buộc");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề là bắt buộc").MaximumLength(300);
        RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ");
        RuleFor(x => x.StartTime).Must(d => d > DateTime.MinValue).WithMessage("Thời gian bắt đầu là bắt buộc");
        RuleFor(x => x.EndTime).GreaterThanOrEqualTo(x => x.StartTime).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");
    }
}
