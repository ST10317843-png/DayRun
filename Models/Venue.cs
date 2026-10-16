using System.ComponentModel.DataAnnotations;

namespace DayRun.API.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        public int? EventId { get; set; }
        public Event? Event { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        public string? Province { get; set; }
        public int? Capacity { get; set; }
        public string? Coordinates { get; set; }
    }
}