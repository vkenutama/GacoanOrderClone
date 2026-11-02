using Gacoan.Backend.Dtos;
using Gacoan.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gacoan.Backend.Controllers;

[Authorize]
public class CartController : BaseApiController
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // 1. GET Cart milik Customer yang sedang login
    [HttpGet]
    public async Task<IActionResult> GetMyCart(CancellationToken cancellationToken)
    {
        if (CurrentUserId == Guid.Empty)
        {
            return Unauthorized("User belum terautentikasi.");
        }

        var result = await _cartService.GetMyCartAsync(CurrentUserId, cancellationToken);
        return HandleResult(result);
    }

    // 2. POST Tambah Item ke Cart
    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartDto dto, CancellationToken cancellationToken)
    {
        if (CurrentUserId == Guid.Empty)
        {
            return Unauthorized("User belum terautentikasi.");
        }

        var result = await _cartService.AddItemToCartAsync(CurrentUserId, dto, cancellationToken);
        return HandleResult(result);
    }

    // 3. PUT Update Item (Quantity / Notes)
    [HttpPut("items/{itemId:guid}")]
    public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateCartItemDto dto, CancellationToken cancellationToken)
    {
        if (CurrentUserId == Guid.Empty)
        {
            return Unauthorized("User belum terautentikasi.");
        }

        var result = await _cartService.UpdateCartItemAsync(CurrentUserId, itemId, dto, cancellationToken);
        return HandleResult(result);
    }

    // 4. DELETE Hapus Item Tertentu
    [HttpDelete("items/{itemId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid itemId, CancellationToken cancellationToken)
    {
        if (CurrentUserId == Guid.Empty)
        {
            return Unauthorized("User belum terautentikasi.");
        }

        var result = await _cartService.RemoveCartItemAsync(CurrentUserId, itemId, cancellationToken);
        return HandleResult(result);
    }

    // 5. DELETE Kosongkan Keranjang
    [HttpDelete]
    public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
    {
        if (CurrentUserId == Guid.Empty)
        {
            return Unauthorized("User belum terautentikasi.");
        }

        var result = await _cartService.ClearCartAsync(CurrentUserId, cancellationToken);
        return HandleResult(result);
    }
}
