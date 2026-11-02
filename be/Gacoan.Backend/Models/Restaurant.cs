namespace Gacoan.Backend.Models;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MapsUrl { get; set; } = String.Empty;
    public string Contact { get; set; } = String.Empty;
    public decimal Longitude { get; set; } =Decimal.Zero;
    public decimal Latitude { get; set; } =Decimal.Zero;

    // Restaurant 1->M Operational Hour
    public ICollection<OperationalHour> OperationalHours { get; set; } = new List<OperationalHour>();
}