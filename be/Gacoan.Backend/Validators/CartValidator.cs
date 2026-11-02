using FluentValidation;
using Gacoan.Backend.Dtos;

namespace Gacoan.Backend.Validators;

public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
{
    public AddToCartDtoValidator()
    {
        RuleFor(x => x.RestaurantId)
            .NotEmpty().WithMessage("Restoran wajib dipilih.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Produk wajib dipilih.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Jumlah pesanan minimal 1.");

        RuleFor(x => x.Notes)
            .MaximumLength(300).WithMessage("Catatan maksimal 300 karakter.");
    }
}

public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
{
    public UpdateCartItemDtoValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Jumlah pesanan minimal 1.");

        RuleFor(x => x.Notes)
            .MaximumLength(300).WithMessage("Catatan maksimal 300 karakter.");
    }
}
