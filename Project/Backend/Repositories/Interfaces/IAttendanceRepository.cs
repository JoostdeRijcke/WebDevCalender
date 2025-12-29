using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        // Attendance-specific query methods (Office Attendance)
        Task<bool> IsDateBookedAsync(int userId, DateTime date);
        Task<Attendance?> GetAttendanceByUserAndDateAsync(int userId, DateTime date);
        Task<IEnumerable<Attendance>> GetAttendancesByDateAsync(DateTime date);
        Task<IEnumerable<int>> GetUserIdsByDateAsync(DateTime date);
        Task<IEnumerable<Attendance>> GetAttendancesByUserIdAsync(int userId);
        Task<IEnumerable<DateTime>> GetAttendanceDatesByUserIdAsync(int userId);
    }
}
