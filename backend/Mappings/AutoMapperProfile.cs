using AutoMapper;
using ManagementSystem.DTOs.Auth;
using ManagementSystem.DTOs.Category;
using ManagementSystem.DTOs.Document;
using ManagementSystem.DTOs.Menu;
using ManagementSystem.DTOs.User;
using ManagementSystem.Entities;

namespace ManagementSystem.Mappings;

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
