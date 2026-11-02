using FluentValidation;
using Gacoan.Backend.Dtos;

namespace Gacoan.Backend.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    public CreateRestaurantDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nama restoran wajib diisi.")
            .MaximumLength(150).WithMessage("Nama restoran maksimal 150 karakter.");

        RuleFor(x => x.Contact)
            .NotEmpty().WithMessage("Kontak restoran wajib diisi.")
            .MaximumLength(50).WithMessage("Kontak maksimal 50 karakter.");

        RuleFor(x => x.MapsUrl)
            .MaximumLength(500).WithMessage("Maps URL maksimal 500 karakter.");
    }
}

public class UpdateRestaurantDtoValidator : AbstractValidator<UpdateRestaurantDto>
{
    public UpdateRestaurantDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nama restoran wajib diisi.")
            .MaximumLength(150).WithMessage("Nama restoran maksimal 150 karakter.");

        RuleFor(x => x.Contact)
            .NotEmpty().WithMessage("Kontak restoran wajib diisi.")
            .MaximumLength(50).WithMessage("Kontak maksimal 50 karakter.");
    }
}

public class CreateOperationalHourDtoValidator : AbstractValidator<CreateOperationalHourDto>
{
    public CreateOperationalHourDtoValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .IsInEnum().WithMessage("Hari tidak valid.");

        RuleFor(x => x.CloseTime)
            .GreaterThan(x => x.OpenTime)
            .When(x => !x.IsClosed)
            .WithMessage("Waktu tutup harus lebih besar dari waktu buka.");
    }
}
