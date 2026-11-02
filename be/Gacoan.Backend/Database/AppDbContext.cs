using Gacoan.Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gacoan.Backend.Database;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<OperationalHour> OperationalHours => Set<OperationalHour>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OrderCart> OrderCarts => Set<OrderCart>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 1. User -> OrderCart (1-to-1)
        builder.Entity<User>(entity =>
        {
            entity
                .HasOne(u => u.Cart)
                .WithOne(oc => oc.User)
                .HasForeignKey<OrderCart>(oc => oc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 2. Restaurant -> OperationalHour (1-to-Many)
        builder.Entity<Restaurant>(entity =>
        {
            entity
                .HasMany(r => r.OperationalHours)
                .WithOne(oh => oh.Restaurant)
                .HasForeignKey(oh => oh.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 3. ProductType -> Product (1-to-Many)
        builder.Entity<Product>(entity =>
        {
            entity
                .HasOne(p => p.Type)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.TypeId);
        });

        // 4. OrderCart -> OrderItem & Restaurant
        builder.Entity<OrderCart>(entity =>
        {
            entity
                .HasOne(oc => oc.Restaurant)
                .WithMany()
                .HasForeignKey(oc => oc.RestaurantId);

            entity
                .HasMany(oc => oc.OrderItems)
                .WithOne(oi => oi.OrderCart)
                .HasForeignKey(oi => oi.OrderCartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 5. OrderItem -> Product
        builder.Entity<OrderItem>(entity =>
        {
            entity
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
        });
    }
}