using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(MyContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> SearchEventsAsync(string? title, string? location, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(e => e.Title.Contains(title));

            if (!string.IsNullOrEmpty(location))
                query = query.Where(e => e.Location.Contains(location));

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            return await query.Include(e => e.EventAttendances).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync(DateTime currentDate)
        {
            return await _dbSet
                .Where(e => e.Date >= currentDate)
                .Include(e => e.EventAttendances)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(e => e.Date >= startDate && e.Date <= endDate)
                .Include(e => e.EventAttendances)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<Event?> GetEventWithAttendeesAsync(int eventId)
        {
            return await _dbSet
                .Include(e => e.EventAttendances)
                .ThenInclude(ea => ea.User)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<IEnumerable<Event>> GetApprovedEventsAsync()
        {
            return await _dbSet
                .Where(e => e.AdminApproval == true)
                .Include(e => e.EventAttendances)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetPendingApprovalEventsAsync()
        {
            return await _dbSet
                .Where(e => e.AdminApproval == false)
                .Include(e => e.EventAttendances)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsWithAttendeesAsync()
        {
            return await _dbSet
                .Include(e => e.EventAttendances)
                .ThenInclude(ea => ea.User)
                .ToListAsync();
        }
    }
}
