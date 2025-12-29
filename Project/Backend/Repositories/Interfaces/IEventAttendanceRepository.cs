using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IEventAttendanceRepository : IRepository<EventAttendance>
    {
        // EventAttendance-specific query methods
        Task<IEnumerable<EventAttendance>> GetAttendeesByEventIdAsync(int eventId);
        Task<IEnumerable<EventAttendance>> GetEventsByUserIdAsync(int userId);
        Task<EventAttendance?> GetAttendanceAsync(int userId, int eventId);
        Task<bool> IsUserRegisteredForEventAsync(int userId, int eventId);
        Task<int> GetAttendeeCountByEventIdAsync(int eventId);
        Task<IEnumerable<EventAttendance>> GetAttendeesWithUserDetailsAsync(int eventId);
        Task<IEnumerable<EventAttendance>> GetEventsWithEventDetailsAsync(int userId);
        Task<IEnumerable<EventAttendance>> GetAllWithReviewsAsync();
        Task<IEnumerable<EventAttendance>> GetReviewsByEventIdAsync(int eventId);
    }
}
