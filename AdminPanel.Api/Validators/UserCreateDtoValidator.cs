using AdminPanel.Web.Dtos;
using FluentValidation;

namespace AdminPanel.Api.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim boş olamaz")
                .Length(2, 100).WithMessage("İsim 2 ile 100 karakter arasında olmalı");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermeli")
                .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermeli")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermeli");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email adresi girin");
        }
    }
}
