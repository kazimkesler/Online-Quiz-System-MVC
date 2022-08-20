using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models.Validators;

namespace OnlineQuizSystem.Models
{
    [ModelMetadataType(typeof(UserValidator))]
    public partial class User
    {
        public User()
        {
            Userquestions = new HashSet<UserQuestion>();
        }

        public long Phone { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool? Sex { get; set; } = null;
        public int Birth { get; set; }
        public int EducationLevel { get; set; }
        public string? Program { get; set; }
        public string? Job { get; set; }
        public DateTime Registration { get; set; }
        public DateTime LastActivity { get; set; }

        public virtual ICollection<UserQuestion> Userquestions { get; set; }
    }
}
