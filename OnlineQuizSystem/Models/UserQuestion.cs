using System;
using System.Collections.Generic;

namespace OnlineQuizSystem.Models
{
    public partial class UserQuestion
    {
        public int UserQuestionId { get; set; }
        public long Phone { get; set; }
        public int Question { get; set; }
        public int Answer { get; set; }
        public int Duration { get; set; }

        public virtual User PhoneNavigation { get; set; } = null!;
        public virtual Question QuestionNavigation { get; set; } = null!;
    }
}
