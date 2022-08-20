namespace OnlineQuizSystem.Models.ViewModels
{
    public class UserAnswerVM
    {
        public int QuestionId { get; set; }
        public string Path { get; set; } = null!;
        public int Category { get; set; }
        public int CorrectAnswer { get; set; }
        public int? Answer { get; set; }
        public int Duration { get; set; }
        public int TotalElapsedTime { get; set; }
    }
}
