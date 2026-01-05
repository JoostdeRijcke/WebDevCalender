using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventAttendanceRepository _eventAttendanceRepository;

        public EventService(IEventRepository eventRepository, IEventAttendanceRepository eventAttendanceRepository)
        {
            _eventRepository = eventRepository;
            _eventAttendanceRepository = eventAttendanceRepository;
        }

        public List<DetailedEventDTO> GetAllEvents()
        {
            return _eventRepository.GetAllEventsWithAttendances();
        }


        public DetailedEventDTO? GetEventById(int id)
        {
            var result = _eventRepository.GetEventByIdWithAttendances(id);

            if (result == null)
            {
                Console.WriteLine($"No event found with ID: {id}");
                return null;
            }

            Console.WriteLine($"Event found: {result.Title}");
            return result;
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

            return await _eventRepository.AddEventAsync(eventToAdd);
        }

        public bool UpdateEvent(int id, UpdateEventDTO updatedEvent)
        {
            return _eventRepository.UpdateEvent(id, updatedEvent);
        }


        public bool DeleteEvent(int id)
        {
            return _eventRepository.DeleteEvent(id);
        }

        public List<ReviewDTO> GetAllReviews()
        {
            return _eventAttendanceRepository.GetAllReviewsWithDetails();
        }

        public bool AddReview(EventAttendance review)
        {
            try
            {
                var existingAttendance = _eventAttendanceRepository.GetAttendance(review.UserId, review.EventId);

                if (existingAttendance != null)
                {
                    existingAttendance.Rating = review.Rating;
                    existingAttendance.Feedback = review.Feedback;
                    _eventAttendanceRepository.UpdateAttendance(existingAttendance);
                    _eventAttendanceRepository.SaveChanges();
                    return true;
                }
                else
                {
                    _eventAttendanceRepository.AddAttendance(review);
                    _eventAttendanceRepository.SaveChanges();
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
            return _eventRepository.SearchEvents(title, location, startDate, endDate);
        }
    }
}
