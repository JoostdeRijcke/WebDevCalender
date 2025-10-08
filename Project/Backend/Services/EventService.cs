using CalendifyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Services
{
    public class EventService : IEventService
    {
        private readonly MyContext _context;

        public EventService(MyContext context)
        {
            _context = context;
        }

        public List<DetailedEventDTO> GetAllEvents()
        {
            return _context.Events
                .Include(e => e.EventAttendances) // Include for potential counts, even if null
                .Select(e => new DetailedEventDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Location = e.Location,
                    AdminApproval = e.AdminApproval
                })
                .ToList();
        }


        public DetailedEventDTO? GetEventById(int id)
        {
            var eventEntity = _context.Events
                .Include(e => e.EventAttendances)
                .FirstOrDefault(e => e.Id == id);

            if (eventEntity == null) 
            {
                Console.WriteLine($"No event found with ID: {id}");
                return null;
            }

            Console.WriteLine($"Event found: {eventEntity.Title}");
            return new DetailedEventDTO
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                Location = eventEntity.Location,
                AdminApproval = eventEntity.AdminApproval
            };
        }




        public async Task<Event> AddEvent(DTOEvent newEvent)
        {
            // Map the DTO to the Event entity
            var eventToAdd = new Event
            {
                Id = newEvent.Id,
                Title = newEvent.Title,
                Description = newEvent.Description,
                Date = newEvent.Date,
                StartTime = newEvent.StartTime,
                EndTime = newEvent.EndTime,
                Location = newEvent.Location,
                AdminApproval = newEvent.AdminApproval
            };

            // Add event to the database
            _context.Events.Add(eventToAdd);
            await _context.SaveChangesAsync();

            return eventToAdd; // Return the saved event
        }

        public bool UpdateEvent(int id, UpdateEventDTO updatedEvent)
        {
            var existingEvent = _context.Events.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null) return false;

            // Update fields
            existingEvent.Title = updatedEvent.Title;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.Date = updatedEvent.Date;
            existingEvent.StartTime = updatedEvent.StartTime;
            existingEvent.EndTime = updatedEvent.EndTime;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.AdminApproval = updatedEvent.AdminApproval;

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

        public List<EventAttendance> GetAllReviews()
        {
            return _context.EventAttendances.Include(ea => ea.User).ToList();
        }

        public bool AddReview(EventAttendance review)
        {
            try
            {
                _context.EventAttendances.Add(review);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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
