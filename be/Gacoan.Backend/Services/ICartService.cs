using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;

namespace Gacoan.Backend.Services;

public interface ICartService
{
    Task<ServiceResult<CartDto>> GetMyCartAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CartDto>> AddItemToCartAsync(Guid userId, AddToCartDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<CartDto>> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<CartDto>> RemoveCartItemAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> ClearCartAsync(Guid userId, CancellationToken cancellationToken = default);
}
