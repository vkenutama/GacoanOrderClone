namespace Gacoan.Backend.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public decimal Price { get; set; }

    // Product -> ProductType
    public Guid TypeId { get; set; }
    public ProductType Type { get; set; } = null!;
}