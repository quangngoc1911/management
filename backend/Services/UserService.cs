using AutoMapper;

using MyAPI.DTOs;
using MyAPI.Helpers;
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

    // =========================
    // GET ALL
    // =========================
    public async Task<List<UserDto>> GetUsers()
    {
        var users = await _repo.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    // =========================
    // CREATE
    // =========================
    public async Task<UserDto> CreateUser(UserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        var created = await _repo.AddAsync(user);

        return _mapper.Map<UserDto>(created);
    }

    // =========================
    // PAGING
    // =========================
    public async Task<PagedResult<UserDto>> GetUsersAsync(PageRequest request)
    {
        var total = await _repo.CountAsync();
        var users = await _repo.GetPagedAsync(request.Skip, request.PageSize);

        var data = _mapper.Map<List<UserDto>>(users);

        return PagedResult<UserDto>.From(data, request, total);
    }


    // =========================
    // REGISTER (có business logic)
    // =========================
    public async Task<UserDto> RegisterAsync(RegisterRequest request)
    {
        // Normalize
        var email = StringHelper.NormalizeEmail(request.Email);

        if (!StringHelper.IsValidEmail(email))
            throw new ArgumentException("Email không hợp lệ");

        if (await _repo.EmailExistsAsync(email))
            throw new InvalidOperationException("Email đã được sử dụng");

        // Map cơ bản
        var user = _mapper.Map<User>(request);

        // Business logic
        user.Name = StringHelper.NormalizeWhitespace(request.Name);
        user.Email = email;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.Role = request.Role ?? "Viewer";
        user.IsActive = true;
        user.CreatedAt = DateTime.UtcNow;

        await _repo.AddAsync(user);

        return _mapper.Map<UserDto>(user);
    }
}