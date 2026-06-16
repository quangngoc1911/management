using FluentValidation;

using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Modules.Family.Application.Validators;

public class UpdateMemberRelationshipDtoValidator : AbstractValidator<UpdateMemberRelationshipDto>
{
    public UpdateMemberRelationshipDtoValidator()
    {
        RuleFor(x => x.RelationshipType).IsInEnum().WithMessage("Loại quan hệ không hợp lệ");

        RuleFor(x => x)
            .Must(x => !x.StartedAt.HasValue || !x.EndedAt.HasValue || x.EndedAt.Value >= x.StartedAt.Value)
            .WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
    }
}
