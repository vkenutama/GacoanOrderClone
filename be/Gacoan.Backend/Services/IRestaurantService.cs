using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;

namespace Gacoan.Backend.Services;

public interface IRestaurantService
{
    Task<ServiceResult<List<RestaurantDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<RestaurantDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<RestaurantDto>> CreateAsync(CreateRestaurantDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<RestaurantDto>> UpdateAsync(Guid id, UpdateRestaurantDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<OperationalHourDto>> AddOperationalHourAsync(Guid restaurantId, CreateOperationalHourDto dto, CancellationToken cancellationToken = default);
}
