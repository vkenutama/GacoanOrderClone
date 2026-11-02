namespace Gacoan.Backend.Dtos;

public class CartDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;
    public string? Notes { get; set; }
}

public record AddToCartDto
{
    public Guid RestaurantId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Notes { get; set; }
}

public record UpdateCartItemDto
{
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
