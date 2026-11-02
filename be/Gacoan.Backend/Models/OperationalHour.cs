namespace Gacoan.Backend.Models;

public class OperationalHour
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public bool IsClosed { get; set; }

    // Operational Hour -> Restaurant
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}