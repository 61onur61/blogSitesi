using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blogSitesi.Models.ValidationRules
{
    public class UyeValidator:AbstractValidator<Uye>
    {
        public UyeValidator()
        {
            RuleFor(u => u.kullaniciAd).NotEmpty().WithMessage("Kullanıcı Adı Boş Bırakılamaz!")
                .Length(2, 150).WithMessage("Kullanıcı Adı En Fazla 150 En Az 2 Karakter Olmalıdır.");




            RuleFor(u => u.kullaniciEmail).NotEmpty().WithMessage("Email Adresi Boş Bırakılamaz!")
                .EmailAddress().WithMessage("Email Adress Formatında Giriniz!");


            RuleFor(u => u.kullaniciSifre).NotEmpty().WithMessage("Şife Boş Bırakılamaz!")
                .Length(4, 10).WithMessage("Şifre En Fazla 10 En Az 4 Karakter Olmalıdır.");

            //.Matches("[^a-zA-Z0-9]").WithMessage("Parola Özel Karakter İçermelidir.");


            












        }
    }
}