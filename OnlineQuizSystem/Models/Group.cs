namespace OnlineQuizSystem.Models
{
    public partial class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = null!;
        public string Link { get; set; } = null!;
        public bool State { get; set; }
        public string Pass { get; set; } = null!;
        public int RequiredScore { get; set; }
    }
}
