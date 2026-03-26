using AutoMapper;

using MyAPI.DTOs;
using MyAPI.Models;

namespace MyAPI.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<UserDto, User>();

        // Nếu muốn ignore field đặc biệt (optional)
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}