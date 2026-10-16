using System.ComponentModel.DataAnnotations;

namespace DayRun.API.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event? Event { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal EntryFee { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}