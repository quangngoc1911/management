using FluentValidation;

using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Modules.Family.Application.Validators;

public class UpsertMemberProfileDtoValidator : AbstractValidator<UpsertMemberProfileDto>
{
    public UpsertMemberProfileDtoValidator()
    {
        RuleFor(x => x.NationalId).MaximumLength(50).WithMessage("Số CMND/CCCD tối đa 50 ký tự");
        RuleFor(x => x.PassportNo).MaximumLength(50).WithMessage("Số hộ chiếu tối đa 50 ký tự");
        RuleFor(x => x.Nationality).MaximumLength(100);
        RuleFor(x => x.Ethnicity).MaximumLength(100);
        RuleFor(x => x.Religion).MaximumLength(100);
        RuleFor(x => x.BloodType).MaximumLength(10);
        RuleFor(x => x.Occupation).MaximumLength(200);
        RuleFor(x => x.BirthPlace).MaximumLength(300);
        RuleFor(x => x.CurrentAddress).MaximumLength(500);
        RuleFor(x => x.PermanentAddress).MaximumLength(500);
        RuleFor(x => x.EmergencyContactName).MaximumLength(200);
        RuleFor(x => x.EmergencyContactPhone).MaximumLength(20);

        RuleFor(x => x.MaritalStatus)
            .IsInEnum().WithMessage("Tình trạng hôn nhân không hợp lệ")
            .When(x => x.MaritalStatus.HasValue);

        RuleFor(x => x.EducationLevel)
            .IsInEnum().WithMessage("Trình độ học vấn không hợp lệ")
            .When(x => x.EducationLevel.HasValue);

        RuleFor(x => x.HeightCm)
            .InclusiveBetween(0, 300).WithMessage("Chiều cao (cm) không hợp lệ")
            .When(x => x.HeightCm.HasValue);

        RuleFor(x => x.WeightKg)
            .InclusiveBetween(0, 500).WithMessage("Cân nặng (kg) không hợp lệ")
            .When(x => x.WeightKg.HasValue);
    }
}
