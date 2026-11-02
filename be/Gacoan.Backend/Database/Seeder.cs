using Gacoan.Backend.Models;
using Microsoft.EntityFrameworkCore;

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

        await _context.SaveChangesAsync();
    }
}