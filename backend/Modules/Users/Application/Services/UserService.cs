using AutoMapper;

using ManagementSystem.Application.DTOs;
using ManagementSystem.Modules.Users.Application.DTOs;
using ManagementSystem.Modules.Auth.Application.DTOs;
using ManagementSystem.Application.Helpers;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Application.DTOs.Common;
using PageRequest = ManagementSystem.Application.DTOs.Common.PageRequest;

namespace ManagementSystem.Modules.Users.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public UserService(IDateTime dateTime,IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<UserDto>> GetAllUsersAsync(PageRequest request)
    {
        var total = await _repo.CountAsync();
        var users = await _repo.GetPagedAsync(request.Skip, request.PageSize);

        var data = _mapper.Map<List<UserDto>>(users);

        return new PaginatedResultDto<UserDto>
        {
            Items = data,
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _repo.GetByEmailAsync(email);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, Guid createdBy)
    {
        var email = StringHelper.NormalizeEmail(dto.Email);

        if (!StringHelper.IsValidEmail(email))
            throw new ArgumentException("Email không hợp lệ");

        if (await _repo.EmailExistsAsync(email))
            throw new InvalidOperationException("Email đã được sử dụng");

        var user = _mapper.Map<User>(dto);
        user.Email = email;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.CreatedBy = createdBy;
        user.CreatedAt = _dateTime.UtcNow;

        await _repo.CreateAsync(user);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto, Guid updatedBy)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return null;

        if (dto.Email != null && StringHelper.NormalizeEmail(dto.Email) != user.Email)
        {
            var newEmail = StringHelper.NormalizeEmail(dto.Email);
            if (!StringHelper.IsValidEmail(newEmail))
                throw new ArgumentException("Email không hợp lệ");

            if (await _repo.EmailExistsAsync(newEmail, id))
                throw new InvalidOperationException("Email đã được sử dụng bởi người dùng khác");
            user.Email = newEmail;
        }

        _mapper.Map(dto, user);

        if (dto.Password != null)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }
        
        user.UpdatedBy = updatedBy;
        user.UpdatedAt = _dateTime.UtcNow;

        _repo.Update(user);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return false;

        _repo.Delete(user);
        return true;
    }

    public async Task<UserDto> RegisterAsync(RegisterRequest request)
    {
        var email = StringHelper.NormalizeEmail(request.Email);

        if (!StringHelper.IsValidEmail(email))
            throw new ArgumentException("Email không hợp lệ");

        if (await _repo.EmailExistsAsync(email))
            throw new InvalidOperationException("Email đã được sử dụng");

        var user = _mapper.Map<User>(request);

        user.UserName = StringHelper.NormalizeWhitespace(request.Name);
        user.Email = email;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.Status = ManagementSystem.Domain.Enums.EntityStatus.Active;
        user.CreatedAt = _dateTime.UtcNow;

        await _repo.CreateAsync(user);

        return _mapper.Map<UserDto>(user);
    }
}
