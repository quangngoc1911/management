using FluentValidation;

using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Application.Validators;

public class CreateBudgetDtoValidator : AbstractValidator<CreateBudgetDto>
{
    public CreateBudgetDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên ngân sách là bắt buộc").MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0");
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.PeriodType).NotEmpty().WithMessage("Kỳ ngân sách là bắt buộc").MaximumLength(20);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
    }
}
