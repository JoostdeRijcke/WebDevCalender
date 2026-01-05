using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class EventAttendanceRepository : IEventAttendanceRepository
    {
        private readonly MyContext _context;

        public EventAttendanceRepository(MyContext context)
        {
            _context = context;
        }

        public EventAttendance? GetAttendance(int userId, int eventId)
        {
            return _context.EventAttendances
                .FirstOrDefault(ea => ea.UserId == userId && ea.EventId == eventId);
        }

        public int GetAttendeeCount(int eventId)
        {
            return _context.EventAttendances.Count(ea => ea.EventId == eventId);
        }

        public User? GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public Event? GetEventById(int eventId)
        {
            return _context.Events.FirstOrDefault(e => e.Id == eventId);
        }

        public void AddAttendance(EventAttendance attendance)
        {
            _context.EventAttendances.Add(attendance);
        }

        public void RemoveAttendance(EventAttendance attendance)
        {
            _context.EventAttendances.Remove(attendance);
        }

        public IEnumerable<object> GetAttendeesByEventId(int eventId)
        {
            return _context.EventAttendances
                .Where(ea => ea.EventId == eventId)
                .Include(ea => ea.User)
                .Select(ea => new
                {
                    ea.UserId,
                    UserName = $"{ea.User.FirstName} {ea.User.LastName}",
                    ea.AttendedAt
                })
                .ToList();
        }

        public IEnumerable<object> GetEventsByUserId(int userId)
        {
            return _context.EventAttendances
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
        }

        public List<ReviewDTO> GetAllReviewsWithDetails()
        {
            return _context.EventAttendances
                .Where(ea => ea.Rating != null && ea.Rating > 0)
                .Include(ea => ea.User)
                .Include(ea => ea.Event)
                .Select(ea => new ReviewDTO
                {
                    Id = ea.Id,
                    UserId = ea.UserId,
                    EventId = ea.EventId,
                    Rating = ea.Rating ?? 0,
                    Feedback = ea.Feedback ?? "",
                    AttendedAt = ea.AttendedAt,
                    User = new UserDTO
                    {
                        FirstName = ea.User.FirstName,
                        LastName = ea.User.LastName
                    },
                    Event = new EventInfoDTO
                    {
                        Title = ea.Event.Title,
                        Date = ea.Event.Date
                    }
                })
                .ToList();
        }

        public void UpdateAttendance(EventAttendance attendance)
        {
            _context.EventAttendances.Update(attendance);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
