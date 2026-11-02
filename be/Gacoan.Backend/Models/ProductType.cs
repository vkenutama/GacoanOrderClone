namespace Gacoan.Backend.Models;

public class ProductType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}