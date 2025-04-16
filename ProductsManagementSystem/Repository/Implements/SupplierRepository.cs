using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Models.DTO.Supplier;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;
using ProductsManagementSystem.Repository.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Repository.Implements
{
    public class SupplierRepository(AppDbContext dbContext) : GenericRepository<Supplier>(dbContext), ISupplierRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<Supplier>> GetSuppliersAddedInLastDay(int days)
        {
            if (days <= 0)
            {
                throw new ValidationException("Day must be greater than zero.");
            }

            var fromDate = DateTime.Now.AddDays(-days);

            return await _dbContext.Suppliers
                .AsNoTracking()
                .Where(p => p.CreatedDate >= fromDate)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetFilteredSuppliersAsync(SupplierFilterDto filterDto)
        {
            var query = _dbContext.Suppliers
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterDto.Name))
                query = query.Where(s => s.Name.Contains(filterDto.Name));

            if (!string.IsNullOrWhiteSpace(filterDto.Email))
                query = query.Where(s => s.Email == filterDto.Email);

            if (!string.IsNullOrWhiteSpace(filterDto.PhoneNumber))
                query = query.Where(s => s.PhoneNumber.Contains(filterDto.PhoneNumber));

            if (!string.IsNullOrWhiteSpace(filterDto.Address))
                query = query.Where(s => s.Address.Contains(filterDto.Address));

            if (!string.IsNullOrWhiteSpace(filterDto.CompanyName))
                query = query.Where(s => s.CompanyName.Contains(filterDto.CompanyName));

            if (filterDto.CreatedDateFrom.HasValue)
                query = query.Where(s => s.CreatedDate >= filterDto.CreatedDateFrom.Value);

            if (filterDto.CreatedDateTo.HasValue)
                query = query.Where(s => s.CreatedDate <= filterDto.CreatedDateTo.Value);

            if (filterDto.IsActive.HasValue)
                query = query.Where(s => s.IsActive == filterDto.IsActive.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Suppliers
                .AsNoTracking()
                .AnyAsync(s => s.Email.ToUpper() == email.ToUpper());
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber)
        {
            return await _dbContext.Suppliers
                .AsNoTracking()
                .AnyAsync(p => p.PhoneNumber == phoneNumber);
        }
    }
}
