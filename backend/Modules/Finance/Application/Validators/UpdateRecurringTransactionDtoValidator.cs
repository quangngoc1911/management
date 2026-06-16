using FluentValidation;

using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Application.Validators;

public class UpdateRecurringTransactionDtoValidator : AbstractValidator<UpdateRecurringTransactionDto>
{
    public UpdateRecurringTransactionDtoValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Tài khoản là bắt buộc");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên là bắt buộc").MaximumLength(200);
        RuleFor(x => x.Type).NotEmpty().WithMessage("Loại là bắt buộc").MaximumLength(20);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0");
        RuleFor(x => x.Frequency).NotEmpty().WithMessage("Tần suất là bắt buộc").MaximumLength(20);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
    }
}
