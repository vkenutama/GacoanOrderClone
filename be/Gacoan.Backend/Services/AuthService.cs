using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;
using Gacoan.Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Gacoan.Backend.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        // Check if username is already exist        
        var existingUser = await _userManager.FindByNameAsync(dto.UserName);
        if (existingUser != null)
        {
            return ServiceResult<AuthResponseDto>.Fail("Username sudah terdaftar", StatusCodes.Status400BadRequest);
        }

        // Create new user
        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResult<AuthResponseDto>.Fail(errors, 400);
        }

        // Enter the role to user
        await _userManager.AddToRoleAsync(user, dto.Role);

        // Generate JWT Token
        var token = GenerateJwtToken(user, dto.Role);

        var response = new AuthResponseDto
        {
            Token = token,
            UserName = user.UserName!,
            Email = user.Email,
            Role = dto.Role
        };

        return ServiceResult<AuthResponseDto>.Ok(response, "Registrasi berhasil", 200);
    }

    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null)
        {
            return ServiceResult<AuthResponseDto>.Fail("Username atau password salah", 400);
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isValidPassword)
        {
            return ServiceResult<AuthResponseDto>.Fail("Username atau password salah", 400);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? "Customer";

        var token = GenerateJwtToken(user, primaryRole);

        var response = new AuthResponseDto
        {
            Token = token,
            UserName = user.UserName!,
            Email = user.Email!,
            Role = primaryRole
        };

        return ServiceResult<AuthResponseDto>.Ok(response, "Berhasil login", 200);
    }

    public async Task<ServiceResult<UserDetailDto>> GetCurrentUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return ServiceResult<UserDetailDto>.Fail("User tidak ditemukan.", 404);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? "Customer";

        var dto = new UserDetailDto
        {
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            Role = primaryRole,
            IsAdmin = primaryRole == "Admin"
        };

        return ServiceResult<UserDetailDto>.Ok(dto, "Data profil user berhasil diambil.", 200);
    }

    private string GenerateJwtToken(User user, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, role),
            new("FullName", user.FullName ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireDays = double.Parse(_configuration["Jwt:ExpireDays"] ?? "7");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(expireDays),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
