using AutoMapper;
using ManagementSystem.Modules.Auth.Application.DTOs;
using ManagementSystem.Modules.Categories.Application.DTOs;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Modules.Menus.Application.DTOs;
using ManagementSystem.Modules.Users.Application.DTOs;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;
using ManagementSystem.Modules.Menus.Domain.Entities;

namespace ManagementSystem.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User Mappings
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password will be hashed in service
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password will be hashed in service
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password will be hashed in service

        // Document Mappings
        CreateMap<Document, DocumentDto>();
        CreateMap<Document, DocumentListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser!.Name));
        CreateMap<DocumentField, DocumentFieldDto>();

        // Category Mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();

        // Menu Mappings
        CreateMap<Menu, MenuDto>()
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children)); // Recursive mapping
        CreateMap<CreateMenuDto, Menu>();
    }
}