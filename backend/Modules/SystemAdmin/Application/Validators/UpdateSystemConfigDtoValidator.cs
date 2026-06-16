using FluentValidation;

using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Application.Validators;

public class UpdateSystemConfigDtoValidator : AbstractValidator<UpdateSystemConfigDto>
{
    public UpdateSystemConfigDtoValidator()
    {
        RuleFor(x => x.Value).NotEmpty().WithMessage("Giá trị là bắt buộc");
    }
}
