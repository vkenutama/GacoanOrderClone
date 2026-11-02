using Gacoan.Backend.Models;
using DayOfWeek = Gacoan.Backend.Models.DayOfWeek;

namespace Gacoan.Backend.Dtos;

public class RestaurantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MapsUrl { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public List<OperationalHourDto> OperationalHours { get; set; } = new();
}

public record CreateRestaurantDto
{
    public string Name { get; set; } = string.Empty;
    public string MapsUrl { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
}

public record UpdateRestaurantDto
{
    public string Name { get; set; } = string.Empty;
    public string MapsUrl { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
}

public class OperationalHourDto
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public bool IsClosed { get; set; }
    public Guid RestaurantId { get; set; }
}

public record CreateOperationalHourDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public bool IsClosed { get; set; }
}
