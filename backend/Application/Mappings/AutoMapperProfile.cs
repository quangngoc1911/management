using AutoMapper;
using ManagementSystem.Modules.Auth.Application.DTOs;
using ManagementSystem.Modules.Categories.Application.DTOs;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Modules.Users.Application.DTOs;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User Mappings
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // Document Mappings
        CreateMap<Document, DocumentDto>();
        CreateMap<Document, DocumentListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser!.UserName));

        // Category Mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
    }
}
