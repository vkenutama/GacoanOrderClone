namespace Gacoan.Backend.Models;

public class OrderCart
{
    public Guid Id { get; set; }
    
    // OrderCart 1->M OrderItem
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    // OrderCart -> User
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}