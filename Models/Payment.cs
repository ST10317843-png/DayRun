using System.ComponentModel.DataAnnotations;

namespace DayRun.API.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public int EnrolmentId { get; set; }
        public Enrolment? Enrolment { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Pending";
    }
}