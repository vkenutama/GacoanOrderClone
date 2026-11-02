using System.Security.Claims;
using Gacoan.Backend.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gacoan.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(idString, out var id) ? id : Guid.Empty;
        }
    }

    protected string? CurrentUserName => User.FindFirstValue(ClaimTypes.Name);

    protected string? CurrentUserEmail => User.FindFirstValue(ClaimTypes.Email);

    protected string? CurrentUserRole => User.FindFirstValue(ClaimTypes.Role);

    protected bool IsAdmin => User.IsInRole("Admin");

    protected bool IsCustomer => User.IsInRole("Customer");

    protected IActionResult HandleResult<T>(ServiceResult<T> result)
    {
        return StatusCode(result.StatusCode, result);
    }
}
