using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using CalendifyApp.Services;

namespace CalendifyApp.Services
{
    public class EventAttendanceService : IEventAttendanceService
    {
        private readonly IEventAttendanceRepository _eventAttendanceRepository;

        public EventAttendanceService(IEventAttendanceRepository eventAttendanceRepository)
        {
            _eventAttendanceRepository = eventAttendanceRepository;
        }

        public string AttendEvent(AttendanceDto attendance)
        {
            var user = _eventAttendanceRepository.GetUserById(attendance.UserId);
            var eventEntity = _eventAttendanceRepository.GetEventById(attendance.EventId);

            if (user == null || eventEntity == null)
                throw new ArgumentException("User or event not found.");

            if (eventEntity.Date.Add(eventEntity.StartTime) < DateTime.Now)
                throw new ArgumentException("The event has already started or ended.");

            var existingAttendance = _eventAttendanceRepository.GetAttendance(attendance.UserId, attendance.EventId);

            if (existingAttendance != null)
                throw new ArgumentException("You are already registered for this event.");

            if (eventEntity.MaxAttendees.HasValue)
            {
                var currentAttendeeCount = _eventAttendanceRepository.GetAttendeeCount(attendance.EventId);

                if (currentAttendeeCount >= eventEntity.MaxAttendees.Value)
                    throw new ArgumentException("This event is at full capacity.");
            }

            var newAttendance = new EventAttendance
            {
                UserId = attendance.UserId,
                EventId = attendance.EventId,
                AttendedAt = DateTime.Now
            };

            _eventAttendanceRepository.AddAttendance(newAttendance);
            _eventAttendanceRepository.SaveChanges();

            return "Registration successfully recorded.";
        }

        public IEnumerable<object> GetEventAttendees(int eventId)
        {
            var eventEntity = _eventAttendanceRepository.GetEventById(eventId);
            if (eventEntity == null)
            {
                // Return an empty list if the event does not exist
                return Enumerable.Empty<object>();
            }

            return _eventAttendanceRepository.GetAttendeesByEventId(eventId);
        }


        public IEnumerable<object> GetEventsByUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID. Please provide a positive integer value.");

            var user = _eventAttendanceRepository.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            var events = _eventAttendanceRepository.GetEventsByUserId(userId);

            if (!events.Any())
                throw new ArgumentException("No attended events found for this user.");

            return events;
        }

        public void RemoveAttendance(AttendanceDto attendance)
        {
            var attendanceRecord = _eventAttendanceRepository.GetAttendance(attendance.UserId, attendance.EventId);

            if (attendanceRecord == null)
                throw new ArgumentException("Attendance not found.");

            _eventAttendanceRepository.RemoveAttendance(attendanceRecord);
            _eventAttendanceRepository.SaveChanges();
        }
    }
}
