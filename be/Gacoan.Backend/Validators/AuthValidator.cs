using FluentValidation;
using Gacoan.Backend.Configurations;
using Gacoan.Backend.Dtos;
using Gacoan.Backend.Models;

namespace Gacoan.Backend.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username wajib diisi.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(UserConfiguration.MinPasswordLength)
            .WithMessage($"Password minimal {UserConfiguration.MinPasswordLength} karakter.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Format email tidak valid");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Nama lengkap wajib diisi");

        RuleFor(x => x.Role)
            .Must(role => role == "Admin" || role == "Customer")
            .WithMessage("Role hanya boleh 'Admin' atau 'Customer'");
    }
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username wajib diisi.");
        
        RuleFor(x => x.Password)
            .MinimumLength(UserConfiguration.MinPasswordLength)
            .WithMessage($"Password minimal {UserConfiguration.MinPasswordLength} karakter.");
    }
}