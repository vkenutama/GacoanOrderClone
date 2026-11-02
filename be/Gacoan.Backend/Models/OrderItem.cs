namespace Gacoan.Backend.Models;

public class OrderItem
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; } = null;
    public decimal UnitPrice { get; set; }
    
    // OrderItem -> Product
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    // OrderItem -> OrderCart
    public Guid OrderCartId { get; set; }
    public OrderCart OrderCart { get; set; } = null!;
}