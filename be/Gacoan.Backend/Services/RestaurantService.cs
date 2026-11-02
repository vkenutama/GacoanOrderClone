using AutoMapper;
using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;
using Gacoan.Backend.Models;
using Gacoan.Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gacoan.Backend.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRepository<Restaurant> _restaurantRepo;
    private readonly IRepository<OperationalHour> _hourRepo;
    private readonly IMapper _mapper;

    public RestaurantService(
        IRepository<Restaurant> restaurantRepo,
        IRepository<OperationalHour> hourRepo,
        IMapper mapper)
    {
        _restaurantRepo = restaurantRepo;
        _hourRepo = hourRepo;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<RestaurantDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _restaurantRepo.GetQueryable()
            .Include(r => r.OperationalHours)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<RestaurantDto>>(list);
        return ServiceResult<List<RestaurantDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<RestaurantDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var restaurant = await _restaurantRepo.GetQueryable()
            .Include(r => r.OperationalHours)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (restaurant == null)
        {
            return ServiceResult<RestaurantDto>.Fail("Restoran tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        var dto = _mapper.Map<RestaurantDto>(restaurant);
        return ServiceResult<RestaurantDto>.Ok(dto);
    }

    public async Task<ServiceResult<RestaurantDto>> CreateAsync(CreateRestaurantDto dto, CancellationToken cancellationToken = default)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);
        restaurant.Id = Guid.NewGuid();

        await _restaurantRepo.AddAsync(restaurant, cancellationToken);
        await _restaurantRepo.SaveChangesAsync(cancellationToken);

        var resultDto = _mapper.Map<RestaurantDto>(restaurant);
        return ServiceResult<RestaurantDto>.Ok(resultDto, "Restoran berhasil ditambahkan.", StatusCodes.Status201Created);
    }

    public async Task<ServiceResult<RestaurantDto>> UpdateAsync(Guid id, UpdateRestaurantDto dto, CancellationToken cancellationToken = default)
    {
        var restaurant = await _restaurantRepo.GetByIdAsync(id, cancellationToken);
        if (restaurant == null)
        {
            return ServiceResult<RestaurantDto>.Fail("Restoran tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        _mapper.Map(dto, restaurant);
        await _restaurantRepo.SaveChangesAsync(cancellationToken);

        var resultDto = _mapper.Map<RestaurantDto>(restaurant);
        return ServiceResult<RestaurantDto>.Ok(resultDto, "Restoran berhasil diperbarui.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var restaurant = await _restaurantRepo.GetByIdAsync(id, cancellationToken);
        if (restaurant == null)
        {
            return ServiceResult<bool>.Fail("Restoran tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        await _restaurantRepo.RemoveAsync(id, cancellationToken);
        await _restaurantRepo.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.Ok(true, "Restoran berhasil dihapus.");
    }

    public async Task<ServiceResult<OperationalHourDto>> AddOperationalHourAsync(
        Guid restaurantId, 
        CreateOperationalHourDto dto, 
        CancellationToken cancellationToken = default)
    {
        var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId, cancellationToken);
        if (restaurant == null)
        {
            return ServiceResult<OperationalHourDto>.Fail("Restoran tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        var opHour = _mapper.Map<OperationalHour>(dto);
        opHour.Id = Guid.NewGuid();
        opHour.RestaurantId = restaurantId;

        await _hourRepo.AddAsync(opHour, cancellationToken);
        await _hourRepo.SaveChangesAsync(cancellationToken);

        var resultDto = _mapper.Map<OperationalHourDto>(opHour);
        return ServiceResult<OperationalHourDto>.Ok(resultDto, "Jam operasional berhasil ditambahkan.", StatusCodes.Status201Created);
    }
}
