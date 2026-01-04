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


        public DetailedEventDTO? GetEventById(int id)
        {
            var eventEntity = _context.Events
                .Include(e => e.EventAttendances)
                    .ThenInclude(ea => ea.User)
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




        public async Task<Event> AddEvent(DTOEvent newEvent)
        {
            var eventToAdd = new Event
            {
                Id = newEvent.Id,
                Title = newEvent.Title,
                Description = newEvent.Description,
                Date = newEvent.Date,
                StartTime = newEvent.StartTime,
                EndTime = newEvent.EndTime,
                Location = newEvent.Location,
                AdminApproval = newEvent.AdminApproval,
                MaxAttendees = newEvent.MaxAttendees
            };

            _context.Events.Add(eventToAdd);
            await _context.SaveChangesAsync();

            return eventToAdd;
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

        public List<ReviewDTO> GetAllReviews()
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

        public bool AddReview(EventAttendance review)
        {
            try
            {
                var existingAttendance = _context.EventAttendances
                    .FirstOrDefault(ea => ea.UserId == review.UserId && ea.EventId == review.EventId);

                if (existingAttendance != null)
                {
                    existingAttendance.Rating = review.Rating;
                    existingAttendance.Feedback = review.Feedback;
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    _context.EventAttendances.Add(review);
                    _context.SaveChanges();
                    return true;
                }
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
