using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Models.DTO.Auth;
using ProductsManagementSystem.Repository.Interfaces;
using System.Data;

namespace ProductsManagementSystem.Repository.Implements
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }


        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null!;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return new List<ApplicationUser>();

            var users = await _userManager.GetUsersInRoleAsync(roleName);

            return users.ToList();
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();

            return await Task.FromResult(roles); // Convert to Task to keep the async signature
        }

        public async Task<string> UpdateUserRoleAsync(string userId, string newRole)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return "User not found";

            // Get the user's current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Check if the user already has the new role
            if (currentRoles.Count == 1 && currentRoles[0] == newRole)
                return $"The user's current role is already '{newRole}', no update needed";

            // If the user already has the same role but multiple roles exist, clarify
            if (currentRoles.Contains(newRole))
                return $"The user is already in the role '{newRole}'. No changes applied";

            // Remove user from all current roles
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return "Failed to remove the user's current roles";
            }

            // Ensure the new role exists
            var roleExists = await _roleManager.RoleExistsAsync(newRole);
            if (!roleExists)
                return "The specified role does not exist";

            // Add the user to the new role
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
                return "Failed to assign the new role to the user";

            return $"User role updated successfully to '{newRole}'";
        }


        public async Task<string> DeleteUserAsync(string userId)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return "User not found";

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return $"Failed to delete user: {errors}";
            }

            return "User deleted successfully";
        }
    }
}
