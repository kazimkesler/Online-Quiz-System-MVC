using System.ComponentModel.DataAnnotations;

namespace OnlineQuizSystem.Models.Validators
{
    public class MessageValidator
    {
        [Required(ErrorMessage = "Telefon numarası giriniz.")]
        [Range(5000000000, 5999999999, ErrorMessage = "Geçerli bir numara giriniz.")]
        public long Phone { get; set; }

        [StringLength(maximumLength: 1024, MinimumLength = 10, ErrorMessage = "Anlamlı bir mesaj giriniz")]
        [Required(ErrorMessage = "Mesaj giriniz.")]

        public string Content { get; set; } = null!;
    }
}
