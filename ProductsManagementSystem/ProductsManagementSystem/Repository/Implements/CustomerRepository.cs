using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Models.DTO.Customer;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;
using ProductsManagementSystem.Repository.Interfaces;

namespace ProductsManagementSystem.Repository.Implements
{
    public class CustomerRepository(AppDbContext dbContext) : GenericRepository<Customer>(dbContext), ICustomerRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<Customer>> GetCustomersBornAfterYearAsync(int year)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .Where(c => c.DateOfBirth!.Value.Year > year)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetFilteredCustomersAsync(CustomerFilterDto filterDto)
        {
            var query = _dbContext.Customers
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                query = query.Where(c => c.FirstName.Contains(filterDto.Name) || c.LastName.Contains(filterDto.Name));
            }

            if (!string.IsNullOrEmpty(filterDto.Email))
            {
                query = query.Where(c => c.Email.Contains(filterDto.Email));
            }

            if (!string.IsNullOrEmpty(filterDto.Phone))
            {
                query = query.Where(c => c.PhoneNumber.Contains(filterDto.Phone));
            }

            if (!string.IsNullOrEmpty(filterDto.Gender))
            {
                query = query.Where(c => c.Gender.ToLower() == filterDto.Gender.ToLower());
            }

            if (filterDto.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == filterDto.IsActive.Value);
            }

            if (filterDto.DobStart.HasValue)
            {
                query = query.Where(c => c.DateOfBirth >= filterDto.DobStart.Value);
            }

            if (filterDto.DobEnd.HasValue)
            {
                query = query.Where(c => c.DateOfBirth <= filterDto.DobEnd.Value);
            }

            if (filterDto.RegStart.HasValue)
            {
                query = query.Where(c => c.RegistrationDate >= filterDto.RegStart.Value);
            }

            if (filterDto.RegEnd.HasValue)
            {
                query = query.Where(c => c.RegistrationDate <= filterDto.RegEnd.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .AnyAsync(s => s.Email.ToUpper() == email.ToUpper());
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .AnyAsync(p => p.PhoneNumber == phoneNumber);
        }
    }
}
