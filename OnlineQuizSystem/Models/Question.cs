namespace OnlineQuizSystem.Models
{
    public partial class Question
    {
        public Question()
        {
            Userquestions = new HashSet<UserQuestion>();
        }

        public int QuestionId { get; set; }
        public string Path { get; set; } = null!;
        public int Category { get; set; }
        public int Answer { get; set; }

        public virtual ICollection<UserQuestion> Userquestions { get; set; }
    }
}
