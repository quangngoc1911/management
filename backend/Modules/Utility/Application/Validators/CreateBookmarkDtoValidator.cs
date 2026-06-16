using FluentValidation;

using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Modules.Utility.Application.Validators;

public class CreateBookmarkDtoValidator : AbstractValidator<CreateBookmarkDto>
{
    public CreateBookmarkDtoValidator()
    {
        RuleFor(x => x.EntityType).NotEmpty().WithMessage("Loại đối tượng là bắt buộc").MaximumLength(50);
        RuleFor(x => x.EntityId).NotEmpty().WithMessage("Đối tượng là bắt buộc");
        RuleFor(x => x.Note).MaximumLength(500);
    }
}
