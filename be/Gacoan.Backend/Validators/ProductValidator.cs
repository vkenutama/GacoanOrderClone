using FluentValidation;
using Gacoan.Backend.Dtos;

namespace Gacoan.Backend.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nama produk wajib diisi.")
            .MaximumLength(150).WithMessage("Nama produk maksimal 150 karakter.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Harga produk harus lebih dari 0.");

        RuleFor(x => x.TypeId)
            .NotEmpty().WithMessage("Kategori produk (TypeId) wajib dipilih.");
    }
}

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nama produk wajib diisi.")
            .MaximumLength(150).WithMessage("Nama produk maksimal 150 karakter.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Harga produk harus lebih dari 0.");

        RuleFor(x => x.TypeId)
            .NotEmpty().WithMessage("Kategori produk (TypeId) wajib dipilih.");
    }
}
