using FluentValidation;

using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Application.Validators;

public class UpdateInvestmentDtoValidator : AbstractValidator<UpdateInvestmentDto>
{
    public UpdateInvestmentDtoValidator()
    {
        RuleFor(x => x.Type).NotEmpty().WithMessage("Loại đầu tư là bắt buộc").MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên khoản đầu tư là bắt buộc").MaximumLength(200);
        RuleFor(x => x.Symbol).MaximumLength(20);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).When(x => x.Quantity.HasValue);
        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0).When(x => x.PurchasePrice.HasValue);
        RuleFor(x => x.CurrentPrice).GreaterThanOrEqualTo(0).When(x => x.CurrentPrice.HasValue);
    }
}
