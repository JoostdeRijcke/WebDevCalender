using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CalendifyApp.Models
{
    public class EventAttendance
    {
        [Key]
        public int Id { get; set; } // Primary Key
        [Required]
        public int UserId { get; set; } // Foreign Key for User
        [Required]
        public int EventId { get; set; } // Foreign Key for Event
        [Required]
        public DateTime AttendedAt { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; } 
        [MaxLength(250, ErrorMessage = "Feedback cannot exceed 250 characters.")]
        public string? Feedback { get; set; } 

        // Navigation Properties
        public User User { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }

    public class EventAttendanceDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
    }

}
