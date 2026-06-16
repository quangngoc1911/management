using FluentValidation;

using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Modules.Utility.Application.Validators;

public class CreateViewHistoryDtoValidator : AbstractValidator<CreateViewHistoryDto>
{
    public CreateViewHistoryDtoValidator()
    {
        RuleFor(x => x.EntityType).NotEmpty().WithMessage("Loại đối tượng là bắt buộc").MaximumLength(50);
        RuleFor(x => x.EntityId).NotEmpty().WithMessage("Đối tượng là bắt buộc");
        RuleFor(x => x.DurationSeconds).GreaterThanOrEqualTo(0).When(x => x.DurationSeconds.HasValue);
    }
}
