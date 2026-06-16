using FluentValidation;

using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Modules.Medical.Application.Validators;

public class UpdateHealthMetricDtoValidator : AbstractValidator<UpdateHealthMetricDto>
{
    public UpdateHealthMetricDtoValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("Thành viên là bắt buộc");
        RuleFor(x => x.MetricType).NotEmpty().WithMessage("Loại chỉ số là bắt buộc").MaximumLength(50);
        RuleFor(x => x.Unit).NotEmpty().WithMessage("Đơn vị là bắt buộc").MaximumLength(20);
        RuleFor(x => x.MeasuredAt).Must(d => d > DateTime.MinValue).WithMessage("Thời điểm đo là bắt buộc");
    }
}
