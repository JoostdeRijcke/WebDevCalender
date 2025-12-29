using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using CalendifyApp.Services;

namespace CalendifyApp.Services
{
    public class EventAttendanceService : IEventAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventAttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string AttendEvent(AttendanceDto attendance)
        {
            var user = _unitOfWork.Users.GetByIdAsync(attendance.UserId).Result;
            var eventEntity = _unitOfWork.Events.GetByIdAsync(attendance.EventId).Result;

            if (user == null || eventEntity == null)
                throw new ArgumentException("User or event not found.");

            if (eventEntity.Date.Add(eventEntity.StartTime) < DateTime.Now)
                throw new ArgumentException("The event has already started or ended.");

            var existingAttendance = _unitOfWork.EventAttendances
                .GetAttendanceAsync(attendance.UserId, attendance.EventId).Result;

            if (existingAttendance != null)
                throw new ArgumentException("You are already registered for this event.");

            var newAttendance = new EventAttendance
            {
                UserId = attendance.UserId,
                EventId = attendance.EventId,
                AttendedAt = DateTime.Now
            };

            _unitOfWork.EventAttendances.AddAsync(newAttendance).Wait();
            _unitOfWork.SaveChangesAsync().Wait();

            return "Registration successfully recorded.";
        }

        public IEnumerable<object> GetEventAttendees(int eventId)
        {
            var eventEntity = _unitOfWork.Events.GetByIdAsync(eventId).Result;
            if (eventEntity == null)
            {
                // Return an empty list if the event does not exist
                return Enumerable.Empty<object>();
            }

            var attendees = _unitOfWork.EventAttendances
                .GetAttendeesWithUserDetailsAsync(eventId).Result
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

            var user = _unitOfWork.Users.GetByIdAsync(userId).Result;
            if (user == null)
                throw new ArgumentException("User not found.");

            var events = _unitOfWork.EventAttendances
                .GetEventsWithEventDetailsAsync(userId).Result
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
            var attendanceRecord = _unitOfWork.EventAttendances
                .GetAttendanceAsync(attendance.UserId, attendance.EventId).Result;

            if (attendanceRecord == null)
                throw new ArgumentException("Attendance not found.");

            _unitOfWork.EventAttendances.DeleteAsync(attendanceRecord).Wait();
            _unitOfWork.SaveChangesAsync().Wait();
        }
    }
}
