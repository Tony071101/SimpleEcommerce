using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Talent> Talents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }

        //Ràng buộc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //USER
            modelBuilder.Entity<User>().ToTable("Users"); // Đổi tên AspNetUsers thành Users
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles"); // Đổi tên AspNetRoles thành Roles
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles"); // Đổi tên AspNetUserRoles thành UserRoles
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            // PRODUCT
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductID);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict); // tránh xóa nhầm cả đống sản phẩm

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Talent)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.TalentID)
                .OnDelete(DeleteBehavior.Restrict);

            // CATEGORY
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryID);

            modelBuilder.Entity<Category>()
                .Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique(); // Không cho trùng tên danh mục

            // TALENT
            modelBuilder.Entity<Talent>()
                .HasKey(t => t.TalentID);

            modelBuilder.Entity<Talent>()
                .Property(t => t.TalentName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Talent>()
                .Property(t => t.TalentGeneration)
                .HasMaxLength(50);

            // ADDRESS
            modelBuilder.Entity<Address>()
                .HasKey(a => a.AddressID);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // ORDER
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            // ORDER DETAIL
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => od.OrderDetailID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price) // Đảm bảo thuộc tính này tồn tại trong OrderDetail
                .HasPrecision(18, 2);
            // CART
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.CartID);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(cu => cu.Carts)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>()
                .HasIndex(c => new { c.UserID, c.ProductID })
                .IsUnique(); // Không cho trùng giỏ
        }
    }
}