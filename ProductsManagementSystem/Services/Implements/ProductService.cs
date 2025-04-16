using AutoMapper;
using ProductsManagementSystem.Models.DTO.Product;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IFileService fileService, IMapper mapper)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();

            if (products == null || !products.Any())
                throw new Exception("There are not Products!");

            return products;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ValidationException("Id must be greater than zero");

            var product = await _productRepository.GetProductAsync(id) ??
                throw new KeyNotFoundException("Product not found");

            return product;
        }

        public async Task AddAsync(CreateProductDto createDto)
        {
            if (createDto == null)
            {
                throw new ValidationException("Input data cannot be null");
            }
            if (!await _productRepository.CategoryExistsAsync(createDto.CategoryId))
            {
                throw new Exception($"Category does not exist.");
            }

            if (!await _productRepository.SupplierExistsAsync(createDto.SupplierId))
            {
                throw new Exception($"Supplier does not exist.");
            }
            if (createDto.CreatedDate > DateTime.UtcNow)
            {
                throw new Exception("Created date cannot be in the future");
            }
            var product = _mapper.Map<Product>(createDto);

            if (createDto.Image != null)
            {
                var imageName = _fileService.SaveFile(createDto.Image, "Products");
                product.ImagePath = imageName;
            }

            await _productRepository.AddAsync(product);
        }

        public async Task<IEnumerable<ProductDto>> GetCategoriesAddedInLastDay(int days)
        {
            if (days <= 0) throw new Exception("Invalid Value");

            var products = await _productRepository.GetProductsAddedInLastDay(days);

            if (products == null || !products.Any())
                throw new Exception("No products Found!");

            return products;
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filterDto)
        {
            if (filterDto.MinPrice < 0 || filterDto.MaxPrice < 0) throw new Exception("Some Inputs Invalid!");

            var products = await _productRepository.GetFilteredProductsAsync(filterDto);
            if (products == null || !products.Any())
                throw new Exception("There no Products");

            return products;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0) throw new Exception("Value of categoryId Invalid!");

            if (!await _productRepository.CategoryExistsAsync(categoryId))
            {
                throw new ValidationException($"Category does not exist.");
            }
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            if (products == null || !products.Any())
                throw new Exception("There no Products");

            return products;
        }

        public async Task DeleteAsync(int id)
        {
            var productDto = await _productRepository.GetByIdAsync(id)
                ?? throw new Exception("Product not found");

            // Delete the image file using FileService
            if (!string.IsNullOrEmpty(productDto.ImagePath))
            {
                await _fileService.DeleteFileAsync(productDto.ImagePath);
            }

            await _productRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, UpdateProductDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Product not found");
            if (!await _productRepository.CategoryExistsAsync(updateDto.CategoryId))
            {
                throw new Exception($"Category does not exist.");
            }

            if (!await _productRepository.SupplierExistsAsync(updateDto.SupplierId))
            {
                throw new Exception($"Supplier does not exist.");
            }

            // Check if all input values are the same as the current values
            if (product.Name == updateDto.Name && product.Price == updateDto.Price
                && product.StockQuantity == updateDto.StockQuantity
               && product.IsAvailable == updateDto.IsAvailable &&
               product.SupplierId == updateDto.SupplierId && product.CategoryId == updateDto.CategoryId)
            {
                throw new Exception("No changes detected, product remains unchanged");
            }

            _mapper.Map(updateDto, product);

            await _productRepository.UpdateAsync(id, product);
        }
    }
}
