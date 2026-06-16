using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Family.Application.Services;
using ManagementSystem.Modules.Family.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Family.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFamilyModule(this IServiceCollection services)
    {
        services.AddScoped<IFamilyMemberService, FamilyMemberService>();
        services.AddScoped<IFamilyMemberRepository, FamilyMemberRepository>();
        services.AddScoped<IMemberProfileService, MemberProfileService>();
        services.AddScoped<IMemberProfileRepository, MemberProfileRepository>();
        services.AddScoped<IMemberRelationshipService, MemberRelationshipService>();
        services.AddScoped<IMemberRelationshipRepository, MemberRelationshipRepository>();
        return services;
    }
}
