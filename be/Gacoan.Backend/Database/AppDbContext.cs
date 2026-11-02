using Gacoan.Backend.Models;
using Microsoft.AspNetCore.Identity;
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

        builder.Entity<Product>(entity =>
        {
            entity
                .HasMany<OrderItem>()
                .WithOne(o => o.Product)
                .HasForeignKey(o => o.ProductId);

            entity
                .HasOne<ProductType>()
                .WithMany(pt => pt.Products)
                .HasForeignKey(pt => pt.TypeId);
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity
                .HasOne<OrderCart>()
                .WithMany(oc => oc.OrderItems)
                .HasForeignKey(oc => oc.OrderCartId);
        });

        builder.Entity<Restaurant>(entity =>
        {
            entity
                .HasMany<OrderCart>()
                .WithOne(oc => oc.Restaurant)
                .HasForeignKey(oc => oc.RestaurantId);

            entity
                .HasMany<OperationalHour>()
                .WithOne(oh => oh.Restaurant)
                .HasForeignKey(oh => oh.RestaurantId);
        });

        builder.Entity<User>(entity =>
        {
            entity
                .HasOne<OrderCart>()
                .WithOne(oc => oc.User)
                .HasForeignKey<OrderCart>(oc => oc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}