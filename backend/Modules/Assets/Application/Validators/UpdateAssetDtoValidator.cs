using FluentValidation;

using ManagementSystem.Modules.Assets.Application.DTOs;

namespace ManagementSystem.Modules.Assets.Application.Validators;

public class UpdateAssetDtoValidator : AbstractValidator<UpdateAssetDto>
{
    public UpdateAssetDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên tài sản là bắt buộc").MaximumLength(300);
        RuleFor(x => x.AssetType).NotEmpty().WithMessage("Loại tài sản là bắt buộc").MaximumLength(50);
        RuleFor(x => x.Currency).MaximumLength(10);
        RuleFor(x => x.Location).MaximumLength(300);
        RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ");
        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0).When(x => x.PurchasePrice.HasValue);
    }
}
