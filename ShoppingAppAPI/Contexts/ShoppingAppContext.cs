using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DB_Models;

namespace ShoppingAppAPI.Contexts
{
    public class ShoppingAppContext : DbContext
    {
        public ShoppingAppContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Customer>  Customers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set;  }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
             .HasOne(u => u.Customer)
             .WithOne()
             .HasForeignKey<Customer>(c => c.UserID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
             .HasOne(u => u.Seller)
             .WithOne()
             .HasForeignKey<Seller>(c => c.UserID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Seller)
                .WithMany(s => s.OrderDetails)
                .HasForeignKey(od => od.SellerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments) 
                .HasForeignKey(p => p.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne(cu => cu.Cart)
                .HasForeignKey<Cart>(c => c.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<CartItem>()
            //    .HasOne(ci => ci.Product)
            //    .WithMany(p => p.CartItems)
            //    .HasForeignKey(ci => ci.ProductID);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.Order)
            //    .WithOne(o => o.SuccessPayment)
            //    .HasForeignKey<Payment>(p => p.OrderID);

            modelBuilder.Entity<Refund>()
                .HasOne(r => r.Order)
                .WithOne(o => o.Refund)
                .HasForeignKey<Refund>(r => r.OrderID)
                .OnDelete(DeleteBehavior.Restrict);
        }



    }
}
