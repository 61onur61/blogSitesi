using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blogSitesi.Models.ValidationRules
{
    public class iletisimValidator : AbstractValidator<Mesaj>
    {
        public iletisimValidator()
        {
            RuleFor(m => m.Gonderen).NotEmpty().WithMessage("Gonderen Boş Bırakılamaz!")
                .Length(2, 150).WithMessage("Gonderen En Fazla 150 En Az 2 Karakter Olmalıdır.");

            RuleFor(m => m.Icerik).NotEmpty().WithMessage("İçerik Boş Bırakılamaz!")
                .Length(2, 1500).WithMessage("İçerik En Fazla 1500 En Az 2 Karakter Olmalıdır.");

            RuleFor(m => m.Konu).NotEmpty().WithMessage("Konu Boş Bırakılamaz!")
                .Length(2, 150).WithMessage("Konu En Fazla 150 En Az 2 Karakter Olmalıdır.");

        }
    }
}