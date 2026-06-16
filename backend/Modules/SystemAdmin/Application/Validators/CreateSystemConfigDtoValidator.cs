using FluentValidation;

using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Application.Validators;

public class CreateSystemConfigDtoValidator : AbstractValidator<CreateSystemConfigDto>
{
    public CreateSystemConfigDtoValidator()
    {
        RuleFor(x => x.Key).NotEmpty().WithMessage("Khoá cấu hình là bắt buộc").MaximumLength(200);
        RuleFor(x => x.Value).NotEmpty().WithMessage("Giá trị là bắt buộc");
    }
}
