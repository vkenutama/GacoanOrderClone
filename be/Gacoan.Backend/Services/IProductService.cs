using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;

namespace Gacoan.Backend.Services;

public interface IProductService
{
    Task<ServiceResult<List<ProductDto>>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDto>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDto>> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDto>> UpdateProductAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
}
