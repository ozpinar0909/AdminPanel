using AdminPanel.Web.Dtos;
using FluentValidation;

namespace AdminPanel.Webapi.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim boş olamaz")
                .Length(2, 100).WithMessage("İsim 2 ile 100 karakter arasında olmalı");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email adresi girin");
        }
    }
}
