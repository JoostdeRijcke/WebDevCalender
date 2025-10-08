using CalendifyApp.Models; // For DetailedEventDTO, DTOEvent, UpdateEventDTO, Event, EventAttendance
using System.Collections.Generic; // For List<T>
using System; // For DateTime

namespace CalendifyApp.Services
{
    public interface IEventService
    {
        List<DetailedEventDTO> GetAllEvents(); // Return DTOEvent instead of Event
        DetailedEventDTO? GetEventById(int id); // Return DTOEvent instead of Event
        Task<Event> AddEvent(DTOEvent newEvent);
        bool UpdateEvent(int id, UpdateEventDTO updatedEvent); // Updated signature
        bool DeleteEvent(int id);
        List<EventAttendance> GetAllReviews();
        bool AddReview(EventAttendance review);
        List<Event> SearchEvents(string? title, string? location, DateTime? startDate, DateTime? endDate);
    }
}
