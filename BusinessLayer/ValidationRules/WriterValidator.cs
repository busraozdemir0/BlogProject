using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    //örnek validator kuralları (writer adlı tablo için yazılmıştı)

    //public class WriterValidator:AbstractValidator<Writer>
    //{
    //    public WriterValidator()
    //    {
    //        RuleFor(x => x.WriterName).NotEmpty().WithMessage("Yazar adı soyadı kısmı boş geçilemez.");
    //        RuleFor(x => x.WriterMail).NotEmpty().WithMessage("Mail adresi boş geçilemez.");
    //        RuleFor(x => x.WriterPassword).NotEmpty().WithMessage("Şifre alanı boş geçilemez.").Must(IsPasswordValid).WithMessage("Şifreniz en az sekiz karakter olmalı, en az bir harf ve bir sayı içermelidir!");
    //        RuleFor(x => x.WriterPassword).Equal(x=>x.ConfirmPassword).WithMessage("Şifreler eşleşmiyor. Lütfen tekrar girin!");
    //        RuleFor(x => x.WriterName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın.");
    //        RuleFor(x => x.WriterName).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterlik veri girişi yapın.");
    //    }
    //    private bool IsPasswordValid(string arg)
    //    {
    //        Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
    //        return regex.IsMatch(arg);
    //    }
    //}
}
