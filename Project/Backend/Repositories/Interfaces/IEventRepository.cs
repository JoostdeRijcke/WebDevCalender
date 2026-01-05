using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IEventRepository
    {
        List<DetailedEventDTO> GetAllEventsWithAttendances();
        DetailedEventDTO? GetEventByIdWithAttendances(int id);
        Event? GetEventById(int id);
        Task<Event> AddEventAsync(Event eventEntity);
        bool UpdateEvent(int id, UpdateEventDTO updatedEvent);
        bool DeleteEvent(int id);
        List<Event> SearchEvents(string? title, string? location, DateTime? startDate, DateTime? endDate);
    }
}
