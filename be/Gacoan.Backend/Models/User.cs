using Microsoft.AspNetCore.Identity;

namespace Gacoan.Backend.Models;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }= String.Empty;

    // User -> OrderCart
    public OrderCart? Cart { get; set; } = null!;
}

