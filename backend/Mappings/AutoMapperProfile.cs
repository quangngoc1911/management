using AutoMapper;
using MyAPI.DTOs;
using MyAPI.Models;

namespace MyAPI.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
    }
}