using AutoMapper;
using ProductsManagementSystem.Models.DTO.Customer;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Services.Implements
{
    public class CustomerService(ICustomerRepository customerRepository, IMapper mapper) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            if (customers == null || !customers.Any())
            {
                throw new Exception("No customers found!");
            }

            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);

            return customersDto;
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("Id must be greater than zero.");
            }
            var customer = await _customerRepository.GetByIdAsync(id)
                ?? throw new Exception($"Customer not found!");
            var customerDto = _mapper.Map<CustomerDto>(customer);

            return customerDto;
        }

        public async Task AddAsync(CustomerCreateDto customerCreateDto)
        {

            if (customerCreateDto == null)
            {
                throw new ValidationException("Input data cannot be null");
            }
            if (await _customerRepository.EmailExistsAsync(customerCreateDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            if (await _customerRepository.PhoneExistsAsync(customerCreateDto.PhoneNumber))
            {
                throw new InvalidOperationException("Phone Number already exists");
            }

            var customer = _mapper.Map<Customer>(customerCreateDto);
            await _customerRepository.AddAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("ID must be greater than zero.");
            }

            var customer = await _customerRepository.GetByIdAsync(id) ??
               throw new KeyNotFoundException($"Customer not found");

            await _customerRepository.DeleteAsync(id);
        }



        public async Task<IEnumerable<CustomerDto>> GetCustomersBornAfterYearAsync(int year)
        {
            var customers = await _customerRepository.GetCustomersBornAfterYearAsync(year) ??
                throw new Exception("No Customers Founded!");

            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return customersDto;
        }

        public async Task UpdateAsync(int id, CustomerUpdateDto customerUpdateDto)
        {
            if (id <= 0)
            {
                throw new ValidationException("ID must be greater than zero.");
            }
            if (customerUpdateDto == null)
            {
                throw new ValidationException("Input data cannot be null.");
            }

            var existingCustomer = await _customerRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException($"Customer not found.");
            _mapper.Map(customerUpdateDto, existingCustomer);

            await _customerRepository.UpdateAsync(id, existingCustomer);
        }

        public async Task<IEnumerable<CustomerDto>> GetFilteredCustomersAsync(CustomerFilterDto filterDto)
        {
            if (filterDto.DobStart.HasValue && filterDto.DobEnd.HasValue && filterDto.DobStart > filterDto.DobEnd)
            {
                throw new ValidationException("Start date of birth cannot be greater than end date.");
            }

            if (filterDto.RegStart.HasValue && filterDto.RegEnd.HasValue && filterDto.RegStart > filterDto.RegEnd)
            {
                throw new ValidationException("Start registration date cannot be greater than end date.");
            }

            var customers = await _customerRepository.GetFilteredCustomersAsync(filterDto);

            if (!customers.Any() || customers == null)
            {
                throw new KeyNotFoundException("No customers found with the given filters.");
            }

            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }
    }
}
