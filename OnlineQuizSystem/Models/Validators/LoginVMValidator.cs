using System.ComponentModel.DataAnnotations;

namespace OnlineQuizSystem.Models.Validators
{
    public class LoginVMValidator
    {
        [Required(ErrorMessage = "Kullanıcı adı giriniz.")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Kullanıcı adı giriniz.")]
        public string User { get; set; } = null!;

        [Required(ErrorMessage = "Şifre giriniz.")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Şifre giriniz.")]
        public string Password { get; set; } = null!;
    }
}
