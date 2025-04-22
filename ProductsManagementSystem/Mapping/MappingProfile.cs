using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Models.DTO.Auth;
using ProductsManagementSystem.Models.DTO.Category;
using ProductsManagementSystem.Models.DTO.Customer;
using ProductsManagementSystem.Models.DTO.Order;
using ProductsManagementSystem.Models.DTO.Product;
using ProductsManagementSystem.Models.DTO.Review;
using ProductsManagementSystem.Models.DTO.Supplier;
using ProductsManagementSystem.Models.Entities;
using System.Security.Claims;

namespace ProductsManagementSystem.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();

            //CreateMap<CategoryUpdateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>()
           .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CustomerCreateDto, Customer>().ReverseMap();
            CreateMap<CustomerUpdateDto, Customer>().ReverseMap();


            CreateMap<OrderUpdateDto, Order>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderCreateDto, Order>().ReverseMap();

            CreateMap<RequestRegisterDto, ApplicationUser>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)).ReverseMap(); // Set UserName = Email
            CreateMap<RequestLoginDto, ApplicationUser>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)).ReverseMap(); // Set UserName = Email
            CreateMap<ApplicationUser, UserDto>().ReverseMap();

            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<CreateReviewDto, Review>().ReverseMap();
            CreateMap<UpdateReviewDto, Review>().ReverseMap();

            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<CreateSupplierDto, Supplier>().ReverseMap();
            CreateMap<UpdateSupplierDto, Supplier>().ReverseMap();

            CreateMap<RequestRegisterDto, ApplicationUser>()
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
    /*.ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))*/; // مهم جداً


            CreateMap<IdentityUser, UserDto>().ReverseMap();
            CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();
            CreateMap<IdentityRole, RoleDto>().ReverseMap();
            CreateMap<IdentityUser, UserUpdateDto>().ReverseMap();

            CreateMap<ClaimsPrincipal, UserDto>()
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Identity.Name))
           // You can map other properties from ClaimsPrincipal if needed
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Email)));


        }
    }
}
