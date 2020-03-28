using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blogSitesi.Models.ValidationRules
{
    public class KategoriValidator: AbstractValidator<Kategori>
    {
        public KategoriValidator()
        {
            RuleFor(k => k.kategoriAd).NotEmpty().WithMessage("Kategori Adı Boş Bırakılamaz.")
                .Length(2, 50).WithMessage("Kategori Adı En Fazla 50 En Az 2 Karakter Olmalıdır.");
        }
    }
}