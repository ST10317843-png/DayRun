using System.ComponentModel.DataAnnotations;

namespace DayRun.API.Models
{
    public class Enrolment
    {
        public int EnrolmentId { get; set; }

        [Required]
        public int ParticipantId { get; set; }
        public User? Participant { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Pending";

        public string? BibNumber { get; set; }

        public Result? Result { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}