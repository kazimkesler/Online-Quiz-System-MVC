namespace OnlineQuizSystem.Models
{
    public partial class GlobalExceptionLog
    {
        public int ExceptionId { get; set; }
        public string? Error { get; set; }
        public string? Detail { get; set; }
    }
}
