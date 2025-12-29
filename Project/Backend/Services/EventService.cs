using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<DetailedEventDTO> GetAllEvents()
        {
            var events = _unitOfWork.Events
                .Include(e => e.EventAttendances)
                .ToList();

            return events.Select(e => new DetailedEventDTO
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                Location = e.Location,
                AdminApproval = e.AdminApproval
            }).ToList();
        }


        public DetailedEventDTO? GetEventById(int id)
        {
            var eventEntity = _unitOfWork.Events.GetEventWithAttendeesAsync(id).Result;

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
            await _unitOfWork.Events.AddAsync(eventToAdd);
            await _unitOfWork.SaveChangesAsync();

            return eventToAdd; // Return the saved event
        }

        public bool UpdateEvent(int id, UpdateEventDTO updatedEvent)
        {
            var existingEvent = _unitOfWork.Events.GetByIdAsync(id).Result;
            if (existingEvent == null) return false;

            // Update fields
            existingEvent.Title = updatedEvent.Title;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.Date = updatedEvent.Date;
            existingEvent.StartTime = updatedEvent.StartTime;
            existingEvent.EndTime = updatedEvent.EndTime;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.AdminApproval = updatedEvent.AdminApproval;

            _unitOfWork.Events.UpdateAsync(existingEvent).Wait();
            _unitOfWork.SaveChangesAsync().Wait();
            return true;
        }


        public bool DeleteEvent(int id)
        {
            var result = _unitOfWork.Events.DeleteAsync(id).Result;
            if (!result) return false;

            _unitOfWork.SaveChangesAsync().Wait();
            return true;
        }

        public List<EventAttendance> GetAllReviews()
        {
            return _unitOfWork.EventAttendances.GetAllWithReviewsAsync().Result.ToList();
        }

        public bool AddReview(EventAttendance review)
        {
            try
            {
                _unitOfWork.EventAttendances.AddAsync(review).Wait();
                _unitOfWork.SaveChangesAsync().Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Event> SearchEvents(string? title, string? location, DateTime? startDate, DateTime? endDate)
        {
            return _unitOfWork.Events.SearchEventsAsync(title, location, startDate, endDate).Result.ToList();
        }
    }
}
