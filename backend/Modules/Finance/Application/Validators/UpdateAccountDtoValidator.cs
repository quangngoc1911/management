using FluentValidation;

using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Application.Validators;

public class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
{
    public UpdateAccountDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên tài khoản là bắt buộc")
            .MaximumLength(200);
        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("Loại tài khoản là bắt buộc")
            .MaximumLength(50);
        RuleFor(x => x.AccountNumber).MaximumLength(100);
        RuleFor(x => x.BankName).MaximumLength(200);
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Đơn vị tiền tệ là bắt buộc")
            .MaximumLength(10);
    }
}
