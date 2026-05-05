using ManagementSystem.DTOs.Auth;
using ManagementSystem.DTOs.Common;
using ManagementSystem.DTOs.User;

namespace ManagementSystem.Interfaces;

public interface IUserService
{
    Task<PaginatedResultDto<UserDto>> GetAllUsersAsync(PageRequest request);
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto> CreateUserAsync(CreateUserDto dto, Guid createdBy);
    Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto, Guid updatedBy);
    Task<bool> DeleteUserAsync(Guid id);
    Task<UserDto> RegisterAsync(RegisterRequest request);
}
