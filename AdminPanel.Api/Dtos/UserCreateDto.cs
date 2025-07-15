using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Web.Dtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "İsim 2 ile 100 karakter arasında olmalı.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalı.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
