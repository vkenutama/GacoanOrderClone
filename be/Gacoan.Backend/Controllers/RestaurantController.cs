using Gacoan.Backend.Dtos;
using Gacoan.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gacoan.Backend.Controllers;

public class RestaurantController : BaseApiController
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _restaurantService.GetAllAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _restaurantService.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDto dto, CancellationToken cancellationToken)
    {
        var result = await _restaurantService.CreateAsync(dto, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRestaurantDto dto, CancellationToken cancellationToken)
    {
        var result = await _restaurantService.UpdateAsync(id, dto, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _restaurantService.DeleteAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/operational-hours")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddOperationalHour(Guid id, [FromBody] CreateOperationalHourDto dto, CancellationToken cancellationToken)
    {
        var result = await _restaurantService.AddOperationalHourAsync(id, dto, cancellationToken);
        return HandleResult(result);
    }
}
