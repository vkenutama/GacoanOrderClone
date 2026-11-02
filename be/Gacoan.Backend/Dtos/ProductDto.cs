namespace Gacoan.Backend.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public Guid TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty; // "Mie" "Beverage Cold" Etc
}

public record CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid TypeId { get; set; }
}

public record UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid TypeId { get; set; }
}

public record ProductTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public record CreateProductTypeDto
{
    public string Name { get; set; } = string.Empty;
}