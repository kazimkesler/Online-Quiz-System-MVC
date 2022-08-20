using System.ComponentModel.DataAnnotations;

namespace OnlineQuizSystem.Models.Validators
{
    public class UserValidator
    {
        [Required(ErrorMessage = "Telefon numarası giriniz.")]
        [Range(5000000000, 5999999999, ErrorMessage = "Geçerli bir numara giriniz.")]
        public long Phone { get; set; }

        [RegularExpression("[a-zA-ZçÇüÜşŞöÖğĞİı]+", ErrorMessage = "Adınızı giriniz.")]
        [StringLength(maximumLength: 16, MinimumLength = 2, ErrorMessage = "Adınızı giriniz.")]
        [Required(ErrorMessage = "Adınızı giriniz.")]
        public string FirstName { get; set; } = null!;
        [RegularExpression("[a-zA-ZçÇüÜşŞöÖğĞİı]+", ErrorMessage = "Soyadınızı giriniz.")]
        [StringLength(maximumLength: 16, MinimumLength = 2, ErrorMessage = "Soyadınızı giriniz.")]
        [Required(ErrorMessage = "Soyadınızı giriniz.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Doğum tarihinizi giriniz.")]
        [Range(1950, 2010, ErrorMessage = "Geçerli bir yıl giriniz.")]
        public int Birth { get; set; }

        [Required(ErrorMessage = "Öğrenim durumunuzu giriniz.")]
        [Range(0, 6, ErrorMessage = "Geçerli bir durum giriniz.")]
        public int EducationLevel { get; set; }
    }
}
