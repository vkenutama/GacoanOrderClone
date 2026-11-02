using AutoMapper;
using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;
using Gacoan.Backend.Models;
using Gacoan.Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gacoan.Backend.Services;

public class CartService : ICartService
{
    private readonly IRepository<OrderCart> _cartRepo;
    private readonly IRepository<OrderItem> _itemRepo;
    private readonly IRepository<Product> _productRepo;
    private readonly IRepository<Restaurant> _restaurantRepo;
    private readonly IMapper _mapper;

    public CartService(
        IRepository<OrderCart> cartRepo,
        IRepository<OrderItem> itemRepo,
        IRepository<Product> productRepo,
        IRepository<Restaurant> restaurantRepo,
        IMapper mapper)
    {
        _cartRepo = cartRepo;
        _itemRepo = itemRepo;
        _productRepo = productRepo;
        _restaurantRepo = restaurantRepo;
        _mapper = mapper;
    }

    public async Task<ServiceResult<CartDto>> GetMyCartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cart = await GetActiveCartAsync(userId, cancellationToken);
        if (cart == null)
        {
            return ServiceResult<CartDto>.Ok(new CartDto { UserId = userId }, "Keranjang masih kosong.");
        }

        var dto = _mapper.Map<CartDto>(cart);
        return ServiceResult<CartDto>.Ok(dto);
    }

    public async Task<ServiceResult<CartDto>> AddItemToCartAsync(Guid userId, AddToCartDto dto, CancellationToken cancellationToken = default)
    {
        // 1. Validasi Restoran
        var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId, cancellationToken);
        if (restaurant == null)
        {
            return ServiceResult<CartDto>.Fail("Restoran tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        // 2. Validasi Produk
        var product = await _productRepo.GetByIdAsync(dto.ProductId, cancellationToken);
        if (product == null)
        {
            return ServiceResult<CartDto>.Fail("Produk tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        // 3. Ambil atau Buat Cart
        var cart = await GetActiveCartAsync(userId, cancellationToken);
        if (cart == null)
        {
            cart = new OrderCart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RestaurantId = dto.RestaurantId
            };
            await _cartRepo.AddAsync(cart, cancellationToken);
            await _cartRepo.SaveChangesAsync(cancellationToken);
        }
        else if (cart.RestaurantId != dto.RestaurantId)
        {
            // Jika pindah cabang restoran, kosongkan keranjang sebelumnya
            cart.OrderItems.Clear();
            cart.RestaurantId = dto.RestaurantId;
        }

        // 4. Tambah item atau update kuantitas jika sudah ada
        var existingItem = cart.OrderItems.FirstOrDefault(i => i.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                existingItem.Notes = dto.Notes;
            }
        }
        else
        {
            var newItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderCartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.Price, // Snapshot harga saat ini
                Notes = dto.Notes
            };
            cart.OrderItems.Add(newItem);
        }

        await _cartRepo.SaveChangesAsync(cancellationToken);

        // Reload data dengan include lengkap
        var updatedCart = await GetActiveCartAsync(userId, cancellationToken);
        var responseDto = _mapper.Map<CartDto>(updatedCart);

        return ServiceResult<CartDto>.Ok(responseDto, "Item berhasil ditambahkan ke keranjang.");
    }

    public async Task<ServiceResult<CartDto>> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemDto dto, CancellationToken cancellationToken = default)
    {
        var cart = await GetActiveCartAsync(userId, cancellationToken);
        if (cart == null)
        {
            return ServiceResult<CartDto>.Fail("Keranjang tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        var item = cart.OrderItems.FirstOrDefault(i => i.Id == cartItemId);
        if (item == null)
        {
            return ServiceResult<CartDto>.Fail("Item tidak ditemukan di keranjang.", StatusCodes.Status404NotFound);
        }

        item.Quantity = dto.Quantity;
        if (dto.Notes != null)
        {
            item.Notes = dto.Notes;
        }

        await _cartRepo.SaveChangesAsync(cancellationToken);

        var updatedCart = await GetActiveCartAsync(userId, cancellationToken);
        var responseDto = _mapper.Map<CartDto>(updatedCart);

        return ServiceResult<CartDto>.Ok(responseDto, "Item berhasil diperbarui.");
    }

    public async Task<ServiceResult<CartDto>> RemoveCartItemAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken = default)
    {
        var cart = await GetActiveCartAsync(userId, cancellationToken);
        if (cart == null)
        {
            return ServiceResult<CartDto>.Fail("Keranjang tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        var item = cart.OrderItems.FirstOrDefault(i => i.Id == cartItemId);
        if (item == null)
        {
            return ServiceResult<CartDto>.Fail("Item tidak ditemukan di keranjang.", StatusCodes.Status404NotFound);
        }

        cart.OrderItems.Remove(item);
        await _cartRepo.SaveChangesAsync(cancellationToken);

        var updatedCart = await GetActiveCartAsync(userId, cancellationToken);
        var responseDto = _mapper.Map<CartDto>(updatedCart);

        return ServiceResult<CartDto>.Ok(responseDto, "Item berhasil dihapus dari keranjang.");
    }

    public async Task<ServiceResult<bool>> ClearCartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cart = await GetActiveCartAsync(userId, cancellationToken);
        if (cart == null)
        {
            return ServiceResult<bool>.Ok(true, "Keranjang sudah kosong.");
        }

        cart.OrderItems.Clear();
        await _cartRepo.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.Ok(true, "Keranjang berhasil dikosongkan.");
    }

    private async Task<OrderCart?> GetActiveCartAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _cartRepo.GetQueryable()
            .Include(c => c.Restaurant)
            .Include(c => c.OrderItems)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }
}
