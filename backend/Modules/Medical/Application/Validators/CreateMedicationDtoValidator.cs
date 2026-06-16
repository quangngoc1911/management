using FluentValidation;

using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Modules.Medical.Application.Validators;

public class CreateMedicationDtoValidator : AbstractValidator<CreateMedicationDto>
{
    public CreateMedicationDtoValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("Thành viên là bắt buộc");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên thuốc là bắt buộc").MaximumLength(200);
        RuleFor(x => x.Dosage).MaximumLength(100);
        RuleFor(x => x.Frequency).MaximumLength(100);
        RuleFor(x => x.StartDate).Must(d => d > DateOnly.MinValue).WithMessage("Ngày bắt đầu là bắt buộc");
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
    }
}
