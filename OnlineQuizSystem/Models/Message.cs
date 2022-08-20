using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models.Validators;

namespace OnlineQuizSystem.Models
{
    [ModelMetadataType(typeof(MessageValidator))]
    public partial class Message
    {
        public int MessageId { get; set; }
        public long Phone { get; set; }
        public string Content { get; set; } = null!;
        public DateTime SentDate { get; set; }
        public bool Done { get; set; }
    }
}
