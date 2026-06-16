using FluentValidation;

using ManagementSystem.Modules.Assets.Application.DTOs;

namespace ManagementSystem.Modules.Assets.Application.Validators;

public class CreateAssetValuationDtoValidator : AbstractValidator<CreateAssetValuationDto>
{
    public CreateAssetValuationDtoValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty().WithMessage("Tài sản là bắt buộc");
        RuleFor(x => x.Value).GreaterThanOrEqualTo(0).WithMessage("Giá trị không hợp lệ");
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ValuationMethod).MaximumLength(100);
        RuleFor(x => x.ValuationDate).Must(d => d > DateOnly.MinValue).WithMessage("Ngày định giá là bắt buộc");
    }
}
