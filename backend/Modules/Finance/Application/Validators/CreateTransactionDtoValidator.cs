using FluentValidation;

using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Application.Validators;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionDtoValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Tài khoản là bắt buộc");
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Loại giao dịch là bắt buộc")
            .MaximumLength(20);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0");
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ");
        RuleFor(x => x.TransactionDate)
            .Must(d => d > DateOnly.MinValue).WithMessage("Ngày giao dịch là bắt buộc");
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
