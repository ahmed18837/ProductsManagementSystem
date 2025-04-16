using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Models.Entities;

namespace ProductsManagementSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Constructor 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تخصيص Sequence واحدة لجميع الـ IDs
            modelBuilder.HasSequence<int>("CommonSequence", schema: "dbo")
                .StartsAt(1000)
                .IncrementsBy(5); // 5 الزيادة بمقدار 

            // Id start with 1000 and increase with 5 for all Ids
            modelBuilder.Entity<Category>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Customer>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Order>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Review>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Supplier>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<OrderProduct>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");


            // Seed Roles
            var userId = "c62edd67-cc0c-40eb-aec5-a5aca03f3d48";
            var adminId = "22ab6d2a-1b36-4dce-aa6c-546a4cf92759";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userId,
                    ConcurrencyStamp = userId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                },
                new IdentityRole
                {
                    Id = adminId,
                    ConcurrencyStamp = adminId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // Add Super Admin Role

            var superAdminId = "2b83d526-cb7e-473c-b339-c4edcd23017f";

            var role = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = superAdminId,
                    ConcurrencyStamp = superAdminId,
                    Name = "Super Admin",
                    NormalizedName = "Super Admin".ToUpper()
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);


            // Product - Category (Many-to-One)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // If Category is deleted, delete related Products

            // Product - Supplier (Many-to-One)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if related products exist

            // Order - Customer (Many-to-One)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull); // If Customer is deleted, set CustomerId to NULL in Orders

            // Order - Product (Many-to-Many)
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Delete OrderProducts if Order is deleted

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Delete OrderProducts if Product is deleted

            // Review - Product (Many-to-One)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Reviews if Product is deleted

            // Review - Customer (Many-to-One)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.SetNull); // Set CustomerId to NULL if Customer is deleted

        }

    }
}
