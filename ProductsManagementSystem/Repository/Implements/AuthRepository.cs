﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Repository.Interfaces;
using System.Text.RegularExpressions;

namespace ProductsManagementSystem.Repository.Implements
{
    public class AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsEmailValid(string email)
        {
            var emailPattern = @"^[\w-\.]+@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, emailPattern);
        }

        public async Task<bool> IsPhoneNumberValid(string phoneNumber)
        {
            var egyptPhonePattern = @"^\+20\d{10}$";
            return Regex.IsMatch(phoneNumber, egyptPhonePattern);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber)
        {
            return await _userManager.Users
                .AnyAsync(p => p.PhoneNumber == phoneNumber);
        }
    }
}
