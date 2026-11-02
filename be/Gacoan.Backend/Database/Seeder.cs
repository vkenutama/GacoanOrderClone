using Gacoan.Backend.Models;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = Gacoan.Backend.Models.DayOfWeek;

namespace Gacoan.Backend.Database;

public class Seeder
{
    private readonly AppDbContext _context;

    public Seeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // 1. Pastikan Kategori "Mie" sudah ada di database
        var mieCategory = await _context.ProductTypes.FirstOrDefaultAsync(pt => pt.Name == "Mie");
        if (mieCategory == null)
        {
            mieCategory = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Mie"
            };
            await _context.ProductTypes.AddAsync(mieCategory);
            await _context.SaveChangesAsync();
        }

        // 2. Loop untuk seeding "Gacoan lv1" sampai "Gacoan lv4"
        for (int i = 1; i <= 4; i++)
        {
            var productName = $"Gacoan lv{i}";

            // Cek apakah produk sudah ada (mencegah duplikasi data saat aplikasi di-restart)
            var exists = await _context.Products.AnyAsync(p => p.Name == productName);
            if (!exists)
            {
                await _context.Products.AddAsync(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = productName,
                    Price = 12000m,
                    TypeId = mieCategory.Id
                });
            }
        }

        // 3. Pastikan minimal ada 1 outlet Restoran dan Jam Operasional
        var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Mie Gacoan Merr Surabaya");
        if (restaurant == null)
        {
            restaurant = new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = "Mie Gacoan Merr Surabaya",
                Contact = "081234567890",
                MapsUrl = "https://maps.google.com/?q=Mie+Gacoan+Merr",
                Latitude = -7.3112m,
                Longitude = 112.7821m
            };

            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();

            // Jam buka Senin - Minggu 09:00 - 22:00
            var allDays = Enum.GetValues<DayOfWeek>();
            foreach (var day in allDays)
            {
                await _context.OperationalHours.AddAsync(new OperationalHour
                {
                    Id = Guid.NewGuid(),
                    RestaurantId = restaurant.Id,
                    DayOfWeek = day,
                    OpenTime = new TimeOnly(9, 0),
                    CloseTime = new TimeOnly(22, 0),
                    IsClosed = false
                });
            }
        }

        await _context.SaveChangesAsync();
    }
}
