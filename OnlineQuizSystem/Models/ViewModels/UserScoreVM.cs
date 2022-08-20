namespace OnlineQuizSystem.Models.ViewModels
{
    public class UserScoreVM
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int EducationLevel { get; set; }
        public string? Program { get; set; }
        public string? Job { get; set; }
        public double Score { get; set; }
    }
}
