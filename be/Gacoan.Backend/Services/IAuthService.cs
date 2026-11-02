using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;

namespace Gacoan.Backend.Services;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto);
}