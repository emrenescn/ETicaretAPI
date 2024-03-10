using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen ürün adını boş geçmeyiniz.")
                .MaximumLength(150)
                .MinimumLength(3)
                .WithMessage("Ürün adı 3 ile 150 karakter arasında olmalıdır.");
            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .WithMessage("Stock bilgisi boş geçilemez.")
                .Must(s => s >= 0)
                .WithMessage("Stock miktarı 0 dan küçük olamaz.");
            RuleFor(p => p.Price)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fiyat alanı boş geçilemez")
                .Must(p => p >= 0)
                .WithMessage("Fiyat sıfırdan büyük olmalıdır");

                
        }
    }
}
