using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Models.DTO.Category;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Services.Implements
{
    public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                throw new Exception("No categories found!");
            }

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return categoriesDto;
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("Id must be greater than zero.");
            }
            var category = await _categoryRepository.GetByIdAsync(id)
                ?? throw new Exception($"Category not found!");
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return categoryDto;
        }

        public async Task<CategoryDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Category name cannot be null or empty.");
            }

            var category = await _categoryRepository.GetByNameAsync(name) ??
                throw new KeyNotFoundException($"Category with name '{name}' not found.");

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task AddAsync(CategoryCreateDto createDto)
        {
            if (createDto == null)
            {
                throw new ValidationException("Input data cannot be null");
            }
            if (await _categoryRepository.GetByNameAsync(createDto.Name) != null)
            {
                throw new ValidationException($"Category with name '{createDto.Name}' already exists.");
            }

            try
            {
                var category = _mapper.Map<Category>(createDto);
                await _categoryRepository.AddAsync(category);
            }
            catch (DbUpdateException ex)
            {
                // Log full inner exception details
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                throw new Exception($"Database error: {errorMessage}", ex);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("ID must be greater than zero.");
            }

            var category = await _categoryRepository.GetByIdAsync(id) ??
               throw new KeyNotFoundException($"Category not found");

            await _categoryRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto updateDto)
        {
            if (id <= 0)
            {
                throw new ValidationException("ID must be greater than zero.");
            }
            if (updateDto == null)
            {
                throw new ValidationException("Input data cannot be null.");
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException($"Category not found.");

            if (await _categoryRepository.GetByNameAsync(updateDto.Name) != null && existingCategory.Name != updateDto.Name)
            {
                throw new ValidationException($"Another category with name '{updateDto.Name}' already exists.");
            }
            if (existingCategory.Name == updateDto.Name)
            {
                throw new Exception("No Changed, New Category is same Old Category!");
            }

            _mapper.Map(updateDto, existingCategory);

            await _categoryRepository.UpdateAsync(id, existingCategory);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAddedInLastDay(int days)
        {
            if (days <= 0) throw new Exception("Invalid Value");

            var categories = await _categoryRepository.GetCategoriesAddedInLastDay(days);
            if (categories == null || !categories.Any())
                throw new Exception("No categories Found!");

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoriesDto;
        }

        public async Task<IEnumerable<CategoryProductCountDto>> GetCategoryProductCountsAsync()
        {
            var categoryProductCountDto = await _categoryRepository.GetCategoryProductCountsAsync();
            if (!categoryProductCountDto.Any() || categoryProductCountDto == null)
                throw new KeyNotFoundException("No categories or products found");

            return categoryProductCountDto;
        }

        public async Task<IEnumerable<CategoryDto>> GetFilteredCategoriesAsync(string? name, bool? isActive, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                throw new ValidationException("Start date cannot be greater than end date.");
            }

            var categories = await _categoryRepository.GetFilteredCategoriesAsync(name, isActive, startDate, endDate);

            if (!categories.Any() || categories == null)
            {
                throw new KeyNotFoundException("No categories found with the given filters.");
            }

            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
}
