using Gacoan.Backend.Dtos;
using Gacoan.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gacoan.Backend.Controllers;

public class ProductController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // 1. GET ALL (Bisa diakses publik / Customer / Admin)
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _productService.GetAllProductsAsync(cancellationToken);
        return HandleResult(result);
    }

    // 2. GET BY ID
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    // 3. CREATE (Hanya Admin)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
    {
        var result = await _productService.CreateProductAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    // 4. UPDATE (Hanya Admin)
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken)
    {
        var result = await _productService.UpdateProductAsync(id, dto, cancellationToken);
        return HandleResult(result);
    }

    // 5. DELETE (Hanya Admin)
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productService.DeleteProductAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
