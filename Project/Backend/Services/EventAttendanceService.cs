using CalendifyApp.Models;
using Microsoft.EntityFrameworkCore;
using CalendifyApp.Services;

namespace CalendifyApp.Services
{
    public class EventAttendanceService : IEventAttendanceService
    {
        private readonly MyContext _context;

        public EventAttendanceService(MyContext context)
        {
            _context = context;
        }

        public string AttendEvent(AttendanceDto attendance)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == attendance.UserId);
            var eventEntity = _context.Events.FirstOrDefault(e => e.Id == attendance.EventId);

            if (user == null || eventEntity == null)
                throw new ArgumentException("User or event not found.");

            if (eventEntity.Date.Add(eventEntity.StartTime) < DateTime.Now)
                throw new ArgumentException("The event has already started or ended.");

            var existingAttendance = _context.EventAttendances
                .FirstOrDefault(ea => ea.UserId == attendance.UserId && ea.EventId == attendance.EventId);

            if (existingAttendance != null)
                throw new ArgumentException("You are already registered for this event.");

            var newAttendance = new EventAttendance
            {
                UserId = attendance.UserId,
                EventId = attendance.EventId,
                AttendedAt = DateTime.Now
            };

            _context.EventAttendances.Add(newAttendance);
            _context.SaveChanges();

            return "Registration successfully recorded.";
        }

        public IEnumerable<object> GetEventAttendees(int eventId)
        {
            var eventEntity = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (eventEntity == null)
            {
                // Return an empty list if the event does not exist
                return Enumerable.Empty<object>();
            }

            var attendees = _context.EventAttendances
                .Where(ea => ea.EventId == eventId)
                .Include(ea => ea.User)
                .Select(ea => new
                {
                    ea.UserId,
                    UserName = $"{ea.User.FirstName} {ea.User.LastName}",
                    ea.AttendedAt
                })
                .ToList();

            // Return the list, even if it is empty
            return attendees;
        }


        public IEnumerable<object> GetEventsByUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID. Please provide a positive integer value.");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            var events = _context.EventAttendances
                .Where(ea => ea.UserId == userId)
                .Include(ea => ea.Event)
                .Select(ea => new
                {
                    ea.Event.Id,
                    ea.Event.Title,
                    ea.Event.Date,
                    ea.Event.StartTime,
                    ea.Event.EndTime,
                    ea.AttendedAt
                })
                .ToList();

            if (!events.Any())
                throw new ArgumentException("No attended events found for this user.");

            return events;
        }

        public void RemoveAttendance(AttendanceDto attendance)
        {
            var attendanceRecord = _context.EventAttendances
                .FirstOrDefault(ea => ea.UserId == attendance.UserId && ea.EventId == attendance.EventId);

            if (attendanceRecord == null)
                throw new ArgumentException("Attendance not found.");

            _context.EventAttendances.Remove(attendanceRecord);
            _context.SaveChanges();
        }
    }
}
