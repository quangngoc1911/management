using FluentValidation;

using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Modules.Family.Application.Validators;

public class UpdateFamilyMemberDtoValidator : AbstractValidator<UpdateFamilyMemberDto>
{
    public UpdateFamilyMemberDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên là bắt buộc")
            .MaximumLength(200).WithMessage("Họ tên không được vượt quá 200 ký tự");

        RuleFor(x => x.Nickname)
            .MaximumLength(100).WithMessage("Biệt danh không được vượt quá 100 ký tự");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Số điện thoại không được vượt quá 20 ký tự");

        RuleFor(x => x.Email)
            .MaximumLength(255).WithMessage("Email không được vượt quá 255 ký tự")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(500).WithMessage("Đường dẫn ảnh đại diện không được vượt quá 500 ký tự");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Giới tính không hợp lệ")
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.RelationToHead)
            .IsInEnum().WithMessage("Quan hệ với chủ hộ không hợp lệ")
            .When(x => x.RelationToHead.HasValue);

        RuleFor(x => x.DateOfBirth)
            .Must(date => date is null || date.Value <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Ngày sinh không được ở tương lai");

        RuleFor(x => x)
            .Must(x => !x.DateOfBirth.HasValue || !x.DateOfDeath.HasValue || x.DateOfDeath.Value >= x.DateOfBirth.Value)
            .WithMessage("Ngày mất phải sau ngày sinh");
    }
}
