using AutoMapper;
using MyAPI.DTOs;
using MyAPI.Interfaces;
using MyAPI.Models;

namespace MyAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _repo.GetAll();
    }

    public async Task<User> CreateUser(UserDTO dto)
    {
        var user = _mapper.Map<User>(dto);

        return await _repo.Add(user);
    }
}