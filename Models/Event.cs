using DayRun.Models;
using System.ComponentModel.DataAnnotations;

namespace DayRun.API.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        public int OrganiserId { get; set; }
        public User? Organiser { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Distance { get; set; } = string.Empty;

        [Required]
        public string EventType { get; set; } = "Run";

        public int? MaxParticipants { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<Category> Categories { get; set; } = new List<Category>();
        public Venue? Venue { get; set; }
    }
}