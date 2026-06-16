using FluentValidation;

using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Modules.Family.Application.Validators;

public class CreateMemberRelationshipDtoValidator : AbstractValidator<CreateMemberRelationshipDto>
{
    public CreateMemberRelationshipDtoValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("Thành viên là bắt buộc");
        RuleFor(x => x.RelatedMemberId).NotEmpty().WithMessage("Thành viên liên quan là bắt buộc");
        RuleFor(x => x.RelationshipType).IsInEnum().WithMessage("Loại quan hệ không hợp lệ");

        RuleFor(x => x.RelatedMemberId)
            .NotEqual(x => x.MemberId)
            .WithMessage("Không thể tạo quan hệ với chính mình");

        RuleFor(x => x)
            .Must(x => !x.StartedAt.HasValue || !x.EndedAt.HasValue || x.EndedAt.Value >= x.StartedAt.Value)
            .WithMessage("Ngày kết thúc phải sau ngày bắt đầu");
    }
}
