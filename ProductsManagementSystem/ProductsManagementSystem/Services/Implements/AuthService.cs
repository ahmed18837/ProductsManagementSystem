using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Helper;
using ProductsManagementSystem.Models.DTO.Auth;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductsManagementSystem.Services.Implements
{
    public class AuthService(IAuthRepository authRepository, UserManager<ApplicationUser> manager
              , RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<JWT> jwt, IEmailService emailService) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly UserManager<ApplicationUser> _manager = manager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IEmailService _emailService = emailService;
        private readonly IMapper _mapper = mapper;
        private readonly JWT _jwt = jwt.Value;


        public async Task UserRegisterAsync(RequestRegisterDto request)
        {
            var existingUser = await _manager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("A user with this email already exists");

            if (await _authRepository.PhoneExistsAsync(request.PhoneNumber))
                throw new InvalidOperationException("Phone Number  already exists!");

            var appUser = _mapper.Map<ApplicationUser>(request);

            var identityResult = await _manager.CreateAsync(appUser, request.Password);
            if (!identityResult.Succeeded)
            {
                //throw new Exception("InValid Email Or Password!");
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }
            identityResult = await _manager.AddToRoleAsync(appUser, "User");
            if (!identityResult.Succeeded)
            {
                //throw new Exception("Failed to assign the role 'User'");
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign the role 'User': {errors}");
            }
        }

        public async Task AdminRegisterAsync(RequestRegisterDto request)
        {
            var existingUser = await _manager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("A Admin with this email already exists");

            if (await _authRepository.PhoneExistsAsync(request.PhoneNumber))
                throw new InvalidOperationException("Phone Number already exists!");

            var appAdmin = _mapper.Map<ApplicationUser>(request);

            var identityResult = await _manager.CreateAsync(appAdmin, request.Password);
            if (!identityResult.Succeeded)
            {
                //throw new Exception("InValid Email Or Password!");
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }
            identityResult = await _manager.AddToRoleAsync(appAdmin, "Admin");
            if (!identityResult.Succeeded)
            {
                //throw new Exception("Failed to assign the role 'User'");
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign the role 'User': {errors}");
            }
        }

        public async Task<ResponseDto> LoginAsync(RequestLoginDto request)
        {
            var authModel = new ResponseDto();

            var user = await _manager.FindByEmailAsync(request.Email) ??
                throw new Exception("UserName or Password is incorrect");

            var isValidPassword = await _manager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword)
                throw new Exception("UserName or Password is incorrect");

            var roles = await _manager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
                throw new Exception("User has no assigned roles");

            var token = await CreateJwtToken(user);
            string encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            authModel.AccessToken = encodedToken;

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _manager.UpdateAsync(user);
            }

            return authModel;

            //        string subject = "Your Login Token";
            //        string body = $@"
            //    <h2>Your JWT Token</h2>
            //    <div style='background-color: #f4f4f9; padding: 15px; border-radius: 8px; margin-bottom: 10px;'>
            //        <p style='font-family: Arial, sans-serif; color: #333;'>Below is your access token. Please keep it safe and use it for authentication:</p>
            //        <code style='display: block; background-color: #333; color: #fff; padding: 10px; border-radius: 5px; overflow-x: auto;'>{encodedToken}</code>
            //    </div>
            //    <p style='font-family: Arial, sans-serif; color: #333;'>Use this token for authentication when making requests to the API.</p>
            //";

            //        bool emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);
            //        if (!emailSent)
            //            return "Login successful, but failed to send token via email";

            //        return "Login successful. Token sent to your email";
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var authModel = new AuthResponseDto();

            var user = await _manager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = "Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.Now;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _manager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            var roles = await _manager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }

        public async Task RevokeTokenAsync(string token)
        {
            var user = await _manager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                throw new Exception("User Not Founded!");

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                throw new Exception("Token Not Active!");

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _manager.UpdateAsync(user);
        }

        public async Task<string> AssignRoleAsync(string email, string roleName)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new Exception("This Role does not exist");

            var result = await _manager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role: {errors}");
            }

            return $"Role '{roleName}' assigned to user '{email}' successfully";
        }

        public async Task<string> ForgetPasswordAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            var resetCode = GenerateCode();

            user.ResetCode = resetCode;
            await _manager.UpdateAsync(user);

            var subject = "Password Reset Code";
            var message = $"Your password reset code is: {resetCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return $"A password reset code has been sent to your email.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email) ??
               throw new Exception("User not found");

            //  التحقق من الرمز
            if (user.ResetCode != model.ResetCode)
                throw new Exception("Invalid reset code");

            // التحقق مما إذا كانت كلمة المرور الجديدة هي نفس القديمة
            var passwordCheck = await _manager.CheckPasswordAsync(user, model.NewPassword);
            if (passwordCheck)
                throw new Exception("New password cannot be the same as the current password.");

            //  إعادة تعيين كلمة المرور
            var resetPassword = await _manager.GeneratePasswordResetTokenAsync(user);

            var result = await _manager.ResetPasswordAsync(user, resetPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to reset password: {errors}");
            }

            //  إزالة الرمز بعد الاستخدام
            user.ResetCode = null;
            await _manager.UpdateAsync(user);

            return "Password has been reset successfully!";
        }

        public async Task<string> Send2FACodeAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            // التحقق مما إذا كان هناك كود موجود ولكنه منتهي الصلاحية
            if (user.TwoFactorCodeExpiration != null && user.TwoFactorCodeExpiration > DateTime.Now)
                return "A valid 2FA code has already been sent. Please check your email.";

            var twoFactorCode = GenerateCode();

            user.TwoFactorCode = twoFactorCode;
            user.TwoFactorCodeExpiration = DateTime.Now.AddMinutes(5);

            await _manager.UpdateAsync(user);

            var subject = "Your 2FA Code";
            var message = $"Your Two-Factor Authentication code is : {twoFactorCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return "A 2FA code has been sent to your email.";
        }

        public async Task<string> Resend2FACodeAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            if (user.TwoFactorSentAt != null && (DateTime.UtcNow - user.TwoFactorSentAt.Value).TotalSeconds < 60)
            {
                throw new Exception("Please wait at least 1 minute before requesting a new code.");
            }

            // التحقق مما إذا كان الحساب مقفلًا حاليًا
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                throw new Exception($"Your account is locked. Try again at {user.LockoutEnd.Value.ToLocalTime()}.");
            }

            // إعادة ضبط المحاولات إذا مرت ساعة
            if (user.LastTwoFactorAttempt != null && (DateTime.UtcNow - user.LastTwoFactorAttempt.Value).TotalHours >= 1)
            {
                user.TwoFactorAttempts = 0;
            }

            if (user.TwoFactorAttempts >= 5)
            {
                throw new Exception("You have exceeded the maximum number of attempts. Please try again later.");
            }

            var newCode = GenerateCode();
            user.TwoFactorCode = newCode;
            user.TwoFactorCodeExpiration = DateTime.UtcNow.AddMinutes(10);

            user.TwoFactorSentAt = DateTime.UtcNow;

            user.TwoFactorAttempts += 1;
            user.LastTwoFactorAttempt = DateTime.UtcNow;

            await _manager.UpdateAsync(user);

            await _emailService.SendEmailAsync(user.Email, "Your 2FA Code", $"Your new 2FA code is: {newCode}");

            return "A new 2FA code has been sent to your email.";
        }

        public async Task<string> Verify2FACodeAsync(Verify2FACodeDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email)
             ?? throw new Exception("User not found");

            // التحقق مما إذا كان الكود قد انتهت صلاحيته
            if (user.TwoFactorCode == null || user.TwoFactorCodeExpiration < DateTime.UtcNow)
                throw new Exception("The 2FA code has expired. Please request a new one.");

            // التحقق من صحة الكود
            if (user.TwoFactorCode != model.Code)
            {
                user.FailedTwoFactorAttempts++;

                // إذا تجاوز 5 محاولات خاطئة، يتم قفل الحساب
                if (user.FailedTwoFactorAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(10); //  قفل الحساب لمدة 10 دقيقة
                    await _manager.UpdateAsync(user);
                    throw new Exception("Too many failed attempts. Your account is locked for 10 minutes.");
                }

                await _manager.UpdateAsync(user);
                throw new Exception("Invalid 2FA code.");
            }

            // Reset the 2FA code after successful verification
            user.FailedTwoFactorAttempts = 0;
            user.TwoFactorAttempts = 0;
            user.LockoutEnd = null;
            user.TwoFactorCode = null;
            user.TwoFactorCodeExpiration = null;

            await _manager.UpdateAsync(user);

            return "2FA verification successful";
        }

        public async Task<string> UnlockUserAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            //  التحقق مما إذا كان الحساب مقفلًا حاليًا
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                user.LockoutEnd = null; // إزالة القفل
                user.FailedTwoFactorAttempts = 0; // إعادة تعيين المحاولات الفاشلة
                await _manager.UpdateAsync(user);
                return "User account has been unlocked successfully.";
            }

            return "User account is not locked.";
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email)
             ?? throw new Exception("User not found");

            // Verify old password
            var isPasswordCorrect = await _manager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isPasswordCorrect)
                throw new Exception("Incorrect Current password");

            // Ensure new password is not the same as the old one
            if (model.CurrentPassword == model.NewPassword)
                throw new Exception("New password cannot be the same as the Current password");

            var result = await _manager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password change failed: {errors}");
            }

            return "Password changed successfully";
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = _manager.Users.ToList();
            if (users == null || !users.Any())
            {
                throw new Exception("No Users Founded!");
            }
            var usersDto = _mapper.Map<List<UserDto>>(users);

            // User لكل  Role لتحديد 
            foreach (var userDto in usersDto)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                userDto.Roles = await _manager.GetRolesAsync(user);
            }

            return usersDto;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email)
                 ?? throw new Exception("User not found");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _manager.GetRolesAsync(user); // تعيين الأدوار يدويًا

            return userDto;
        }

        public async Task<UserDto> GetUserByIdAsync(string Id)
        {
            var user = await _manager.FindByIdAsync(Id)
                 ?? throw new Exception("User not found");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _manager.GetRolesAsync(user);

            return userDto;
        }

        public async Task UpdateUserAsync(string Id, UpdateUserDto model)
        {
            var user = await _manager.FindByIdAsync(Id)
                ?? throw new Exception("User not found");

            if (user.FullName == model.FullName && user.PhoneNumber == model.PhoneNumber
                && user.Email == model.Email)
                throw new Exception("No changes detected. The data is already up to date.");

            _mapper.Map(model, user);

            var result = await _manager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update user: {errors}");
            }
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            if (roles == null || !roles.Any())
                throw new Exception("No Roles Exist!");

            return roles;
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName) ??
                throw new Exception("Role not found");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to delete role: {errors}");
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                throw new Exception("Role does not exist");

            var users = await _manager.GetUsersInRoleAsync(roleName) ??
                throw new Exception("Users does not exist");

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            // User لكل  Role لتحديد 
            foreach (var userDto in usersDto)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                userDto.Roles = await _manager.GetRolesAsync(user);
            }

            return usersDto;
        }

        public async Task UpdateRoleAsync(UpdateRoleDto model)
        {
            var role = await _roleManager.FindByNameAsync(model.OldRoleName) ??
                throw new Exception("Role not found");

            var roleExists = await _roleManager.RoleExistsAsync(model.NewRoleName);
            if (roleExists)
                throw new Exception("New role name already exists");

            role.Name = model.NewRoleName;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update role: {errors}");
            }
        }

        public async Task AddRoleAsync(string roleName)
        {
            // التحقق مما إذا كان الدور موجودًا بالفعل
            if (await _roleManager.RoleExistsAsync(roleName))
                throw new Exception("Role already exists.");

            var concurrencyStamp = Guid.NewGuid().ToString(); // إنشاء قيمة `ConcurrencyStamp` 

            var role = new IdentityRole
            {
                Id = concurrencyStamp,
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = concurrencyStamp
            };

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create role: {errors}");
            }
        }

        public async Task<IEnumerable<string>> GetRolesByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Email is required");
            }

            var user = await _manager.FindByEmailAsync(email) ??
               throw new Exception("User not found");

            var roles = await _manager.GetRolesAsync(user);
            return roles;
        }

        public async Task RemoveUserFromRoleAsync(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
                throw new Exception("Email and role are required.");

            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            if (!await _manager.IsInRoleAsync(user, role))
                throw new Exception("User is not in this Role!");

            var result = await _manager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
                throw new Exception("Failed to remove user from role");
        }

        public async Task DeleteUserAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            var user = await _manager.FindByEmailAsync(email) ??
                throw new KeyNotFoundException("User not found");

            var result = await _manager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to delete user.");
        }

        public async Task AddUserAsync(AddUserDto userDto, string password, string role)
        {

            var user = _mapper.Map<ApplicationUser>(userDto);

            // Check if user already exists
            var existingUser = await _manager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists.");

            user.UserName = user.Email;
            // Check if role exists
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                throw new InvalidOperationException("Specified role does not exist.");

            // Create the user
            var result = await _manager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            // Assign role to user
            var roleResult = await _manager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                throw new InvalidOperationException("User created but failed to assign role.");
        }

        public async Task LogoutAsync(string userEmail)
        {
            var user = await _manager.FindByEmailAsync(userEmail) ??
                throw new Exception("User Not Founded!");

            foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
            {
                token.RevokedOn = DateTime.UtcNow;
            }

            await _manager.UpdateAsync(user);
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _manager.GetClaimsAsync(user);
            var roles = await _manager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role)); // تم التعديل هنا

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(5),
                CreatedOn = DateTime.UtcNow
            };
        }

        private string GenerateCode()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}
