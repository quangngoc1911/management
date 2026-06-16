using FluentValidation;

using ManagementSystem.Modules.Education.Application.DTOs;

namespace ManagementSystem.Modules.Education.Application.Validators;

public class CreateEducationRecordDtoValidator : AbstractValidator<CreateEducationRecordDto>
{
    public CreateEducationRecordDtoValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("Thành viên là bắt buộc");
        RuleFor(x => x.InstitutionName).NotEmpty().WithMessage("Tên cơ sở đào tạo là bắt buộc").MaximumLength(300);
        RuleFor(x => x.Level).NotEmpty().WithMessage("Cấp học là bắt buộc").MaximumLength(50);
        RuleFor(x => x.Major).MaximumLength(200);
        RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ");
        RuleFor(x => x.Gpa).GreaterThanOrEqualTo(0).When(x => x.Gpa.HasValue).WithMessage("GPA không hợp lệ");
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
    }
}
