using FluentValidation;

using ManagementSystem.Modules.Documents.Application.DTOs;

namespace ManagementSystem.Modules.Documents.Application.Validators;

public class UpdateTagDtoValidator : AbstractValidator<UpdateTagDto>
{
    public UpdateTagDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên thẻ là bắt buộc")
            .MaximumLength(100).WithMessage("Tên thẻ không được vượt quá 100 ký tự");

        RuleFor(x => x.Slug)
            .MaximumLength(100).WithMessage("Slug không được vượt quá 100 ký tự");

        RuleFor(x => x.Color)
            .MaximumLength(7).WithMessage("Mã màu không hợp lệ")
            .Matches("^#[0-9a-fA-F]{6}$").WithMessage("Mã màu phải ở dạng hex, ví dụ #2563eb")
            .When(x => !string.IsNullOrWhiteSpace(x.Color));
    }
}
