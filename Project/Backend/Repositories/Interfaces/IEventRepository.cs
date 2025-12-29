using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        // Event-specific query methods
        Task<IEnumerable<Event>> SearchEventsAsync(string? title, string? location, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync(DateTime currentDate);
        Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Event?> GetEventWithAttendeesAsync(int eventId);
        Task<IEnumerable<Event>> GetApprovedEventsAsync();
        Task<IEnumerable<Event>> GetPendingApprovalEventsAsync();
        Task<IEnumerable<Event>> GetEventsWithAttendeesAsync();
    }
}
