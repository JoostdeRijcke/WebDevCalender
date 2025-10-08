namespace CalendifyApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RecurringDays { get; set; } // Assuming this is a string to store days in a specific format.

        public ICollection<EventAttendance> EventAttendances { get; set; }
    }
}