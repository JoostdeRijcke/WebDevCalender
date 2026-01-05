using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class EventRepository : IEventRepository
    {
        private readonly MyContext _context;

        public EventRepository(MyContext context)
        {
            _context = context;
        }

        public List<DetailedEventDTO> GetAllEventsWithAttendances()
        {
            return _context.Events
                .Include(e => e.EventAttendances)
                    .ThenInclude(ea => ea.User)
                .Select(e => new DetailedEventDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Location = e.Location,
                    AdminApproval = e.AdminApproval,
                    MaxAttendees = e.MaxAttendees,
                    EventAttendances = e.EventAttendances.Select(ea => new EventAttendanceDTO
                    {
                        UserId = ea.UserId,
                        UserName = ea.User.FirstName + " " + ea.User.LastName,
                        Rating = ea.Rating ?? 0,
                        Feedback = ea.Feedback ?? ""
                    }).ToList()
                })
                .ToList();
        }

        public DetailedEventDTO? GetEventByIdWithAttendances(int id)
        {
            var eventEntity = _context.Events
                .Include(e => e.EventAttendances)
                    .ThenInclude(ea => ea.User)
                .FirstOrDefault(e => e.Id == id);

            if (eventEntity == null)
            {
                return null;
            }

            return new DetailedEventDTO
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                Location = eventEntity.Location,
                AdminApproval = eventEntity.AdminApproval,
                MaxAttendees = eventEntity.MaxAttendees,
                EventAttendances = eventEntity.EventAttendances.Select(ea => new EventAttendanceDTO
                {
                    UserId = ea.UserId,
                    UserName = ea.User.FirstName + " " + ea.User.LastName,
                    Rating = ea.Rating ?? 0,
                    Feedback = ea.Feedback ?? ""
                }).ToList()
            };
        }

        public Event? GetEventById(int id)
        {
            return _context.Events.FirstOrDefault(e => e.Id == id);
        }

        public async Task<Event> AddEventAsync(Event eventEntity)
        {
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();
            return eventEntity;
        }

        public bool UpdateEvent(int id, UpdateEventDTO updatedEvent)
        {
            var existingEvent = _context.Events.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null) return false;

            existingEvent.Title = updatedEvent.Title;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.Date = updatedEvent.Date;
            existingEvent.StartTime = updatedEvent.StartTime;
            existingEvent.EndTime = updatedEvent.EndTime;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.AdminApproval = updatedEvent.AdminApproval;
            existingEvent.MaxAttendees = updatedEvent.MaxAttendees;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteEvent(int id)
        {
            var eventToDelete = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventToDelete == null) return false;

            _context.Events.Remove(eventToDelete);
            _context.SaveChanges();
            return true;
        }

        public List<Event> SearchEvents(string? title, string? location, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(e => e.Title.Contains(title));

            if (!string.IsNullOrEmpty(location))
                query = query.Where(e => e.Location.Contains(location));

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            return query.Include(e => e.EventAttendances).ToList();
        }
    }
}
