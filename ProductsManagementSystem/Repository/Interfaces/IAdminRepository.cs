using Microsoft.AspNetCore.Identity;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Models.DTO.Auth;

namespace ProductsManagementSystem.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<string> UpdateUserRoleAsync(string userId, string newRole);
        Task<string> DeleteUserAsync(string userId);
    }
}
