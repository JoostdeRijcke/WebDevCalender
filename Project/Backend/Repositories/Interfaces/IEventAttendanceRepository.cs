using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IEventAttendanceRepository
    {
        EventAttendance? GetAttendance(int userId, int eventId);
        int GetAttendeeCount(int eventId);
        User? GetUserById(int userId);
        Event? GetEventById(int eventId);
        void AddAttendance(EventAttendance attendance);
        void RemoveAttendance(EventAttendance attendance);
        IEnumerable<object> GetAttendeesByEventId(int eventId);
        IEnumerable<object> GetEventsByUserId(int userId);
        List<ReviewDTO> GetAllReviewsWithDetails();
        void UpdateAttendance(EventAttendance attendance);
        void SaveChanges();
    }
}
