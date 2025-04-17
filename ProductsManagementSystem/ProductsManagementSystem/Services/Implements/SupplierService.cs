using AutoMapper;
using ProductsManagementSystem.Models.DTO.Supplier;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;

namespace ProductsManagementSystem.Services.Implements
{
    public class SupplierService(ISupplierRepository supplierRepository, IMapper mapper) : ISupplierService
    {

        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();

            if (suppliers == null || !suppliers.Any())
                throw new Exception("There are not suppliers!");

            var suppliersDto = _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
            return suppliersDto;
        }

        public async Task<SupplierDto> GetByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Supplier not found!");

            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            return supplierDto;
        }

        public async Task AddAsync(CreateSupplierDto entity)
        {
            if (entity == null)
                throw new Exception("Supplier Can not be null");

            if (entity.CreatedDate > DateTime.UtcNow)
            {
                throw new Exception("Created date cannot be in the future");
            }
            if (await _supplierRepository.EmailExistsAsync(entity.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            if (await _supplierRepository.PhoneExistsAsync(entity.PhoneNumber))
            {
                throw new InvalidOperationException("Phone Number already exists");
            }

            var supplier = _mapper.Map<Supplier>(entity);
            await _supplierRepository.AddAsync(supplier);
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
                throw new KeyNotFoundException("Supplier not found");
            await _supplierRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliersAddedInLastDay(int days)
        {
            if (days < 0) throw new Exception("Invalid Value");

            var suppliers = await _supplierRepository.GetSuppliersAddedInLastDay(days);
            if (suppliers == null || !suppliers.Any())
                throw new Exception("There are not suppliers!");

            var suppliersDto = _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
            return suppliersDto;
        }

        public async Task UpdateAsync(int id, UpdateSupplierDto entity)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Supplier not found");
            if (await _supplierRepository.EmailExistsAsync(entity.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            if (await _supplierRepository.PhoneExistsAsync(entity.PhoneNumber))
            {
                throw new InvalidOperationException("Phone Number already exists");
            }
            // Check if all input values are the same as the current values
            if (supplier.Name == entity.Name && supplier.Email == entity.Email
                && supplier.PhoneNumber == entity.PhoneNumber && supplier.IsActive == entity.IsActive)
                throw new Exception("No changes detected, supplier remains unchanged");

            _mapper.Map(entity, supplier);
            await _supplierRepository.UpdateAsync(id, supplier);
        }
        public async Task<IEnumerable<SupplierDto>> GetFilteredSuppliersAsync(SupplierFilterDto filterDto)
        {
            var supplier = await _supplierRepository.GetFilteredSuppliersAsync(filterDto);
            if (supplier == null || !supplier.Any())
                throw new Exception("Not there Reviews");
            var suppliersDto = _mapper.Map<IEnumerable<SupplierDto>>(supplier);

            return suppliersDto;
        }
    }
}
