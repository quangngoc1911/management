using FluentValidation;

using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Modules.Medical.Application.Validators;

public class CreateMedicalRecordDtoValidator : AbstractValidator<CreateMedicalRecordDto>
{
    public CreateMedicalRecordDtoValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("Thành viên là bắt buộc");
        RuleFor(x => x.RecordType).NotEmpty().WithMessage("Loại hồ sơ là bắt buộc").MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề là bắt buộc").MaximumLength(300);
        RuleFor(x => x.DoctorName).MaximumLength(200);
        RuleFor(x => x.HospitalName).MaximumLength(300);
        RuleFor(x => x.RecordDate).Must(d => d > DateOnly.MinValue).WithMessage("Ngày khám là bắt buộc");
    }
}
