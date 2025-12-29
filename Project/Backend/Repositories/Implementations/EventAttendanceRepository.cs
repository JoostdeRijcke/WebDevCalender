using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class EventAttendanceRepository : Repository<EventAttendance>, IEventAttendanceRepository
    {
        public EventAttendanceRepository(MyContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EventAttendance>> GetAttendeesByEventIdAsync(int eventId)
        {
            return await _dbSet
                .Where(ea => ea.EventId == eventId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventAttendance>> GetEventsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(ea => ea.UserId == userId)
                .ToListAsync();
        }

        public async Task<EventAttendance?> GetAttendanceAsync(int userId, int eventId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ea => ea.UserId == userId && ea.EventId == eventId);
        }

        public async Task<bool> IsUserRegisteredForEventAsync(int userId, int eventId)
        {
            return await _dbSet
                .AnyAsync(ea => ea.UserId == userId && ea.EventId == eventId);
        }

        public async Task<int> GetAttendeeCountByEventIdAsync(int eventId)
        {
            return await _dbSet
                .Where(ea => ea.EventId == eventId)
                .CountAsync();
        }

        public async Task<IEnumerable<EventAttendance>> GetAttendeesWithUserDetailsAsync(int eventId)
        {
            return await _dbSet
                .Where(ea => ea.EventId == eventId)
                .Include(ea => ea.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventAttendance>> GetEventsWithEventDetailsAsync(int userId)
        {
            return await _dbSet
                .Where(ea => ea.UserId == userId)
                .Include(ea => ea.Event)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventAttendance>> GetAllWithReviewsAsync()
        {
            return await _dbSet
                .Include(ea => ea.User)
                .Include(ea => ea.Event)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventAttendance>> GetReviewsByEventIdAsync(int eventId)
        {
            return await _dbSet
                .Where(ea => ea.EventId == eventId && !string.IsNullOrEmpty(ea.Feedback))
                .Include(ea => ea.User)
                .ToListAsync();
        }
    }
}
