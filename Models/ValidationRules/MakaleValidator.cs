using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blogSitesi.Models.ValidationRules
{
    public class MakaleValidator : AbstractValidator<Makale>
    {
        public MakaleValidator()
        {

            RuleFor(m => m.makaleBaslik).NotEmpty().WithMessage("Makale Adı Boş Bırakılamaz.")
                .Length(2, 250).WithMessage("Makale Adı En Fazla 250 En Az 2 Karakter Olmalıdır.");

            RuleFor(m => m.makaleFoto).NotEmpty().WithMessage("Makale Fotoğrafı Boş Bırakılamaz.");

            RuleFor(m => m.makaleIcerik).NotEmpty().WithMessage("Makale İçeriği Boş Bırakılamaz.");

            RuleFor(m => m.makaleTarih).NotEmpty().WithMessage("Makale Tarihi Boş Bırakılamaz.");


        }
    }
}