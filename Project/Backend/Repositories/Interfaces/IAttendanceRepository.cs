using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<bool> IsDateBookedAsync(int userId, DateTime date);
        Task AddAttendanceAsync(Attendance attendance);
        Task<bool> RemoveAttendanceAsync(int userId, DateTime date);
        Task<List<int>> GetUserIdsByDateAsync(DateTime date);
    }
}
