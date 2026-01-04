using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CalendifyApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
        public bool AdminApproval { get; set; }
        public int? MaxAttendees { get; set; }

        [JsonIgnore]
        public ICollection<EventAttendance> EventAttendances { get; set; }
    }

    public class DetailedEventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
        public bool AdminApproval { get; set; }
        public int? MaxAttendees { get; set; }

        public List<EventAttendanceDTO> EventAttendances { get; set; }
    }

    public class DTOEvent
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public bool AdminApproval { get; set; }
        public int? MaxAttendees { get; set; }
    }

        public class UpdateEventDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public bool AdminApproval { get; set; }
        public int? MaxAttendees { get; set; }
    }

    public class ReviewDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime AttendedAt { get; set; }
        public UserDTO User { get; set; }
        public EventInfoDTO Event { get; set; }
    }

    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EventInfoDTO
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }

}
