using CalendifyApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendifyApp.Services
{
    public interface IOfficeAttendanceService
    {
        Task<bool> IsDateBookedAsync(int userId, DateTime date);
        Task AddAttendanceAsync(Attendance attendance);
        Task<bool> RemoveAttendanceAsync(int userId, DateTime date);
        Task<List<int>> GetUserIdsByDateAsync(DateTime date);
        //Task<List<DateOnly>> GetAttendanceDatesByUserAsync(int userId);
    }
}
