using ProductsManagementSystem.Models.DTO.Auth;

namespace ProductsManagementSystem.Services.Interfaces
{
    public interface IAuthService
    {
        Task UserRegisterAsync(RequestRegisterDto request);

        public Task AdminRegisterAsync(RequestRegisterDto request);
        Task<ResponseDto> LoginAsync(RequestLoginDto request);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task RevokeTokenAsync(string token);
        Task<string> AssignRoleAsync(string email, string roleName);
        Task<string> ForgetPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDto model);
        Task<string> Send2FACodeAsync(string email);
        Task<string> Verify2FACodeAsync(Verify2FACodeDto model);
        public Task<string> UnlockUserAsync(string email);
        Task<string> ChangePasswordAsync(ChangePasswordDto model);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(string Id);
        Task UpdateUserAsync(string Id, UpdateUserDto model);

        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<IEnumerable<string>> GetRolesByEmailAsync(string email);
        Task DeleteRoleAsync(string roleName);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName);

        public Task UpdateRoleAsync(UpdateRoleDto model);
        Task<string> Resend2FACodeAsync(string email);

        Task AddRoleAsync(string roleName);
        Task RemoveUserFromRoleAsync(string email, string role);
        Task AddUserAsync(AddUserDto userDto, string password, string role);
        Task DeleteUserAsync(string email);

        Task LogoutAsync(string userEmail);
    }
}
