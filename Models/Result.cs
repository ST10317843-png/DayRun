using System.ComponentModel.DataAnnotations;

namespace DayRun.API.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        [Required]
        public int EnrolmentId { get; set; }
        public Enrolment? Enrolment { get; set; }

        public TimeSpan? FinishTime { get; set; }
        public int? Position { get; set; }
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}