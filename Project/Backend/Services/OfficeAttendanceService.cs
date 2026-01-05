using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendifyApp.Services
{
    public class OfficeAttendanceService : IOfficeAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public OfficeAttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<bool> IsDateBookedAsync(int userId, DateTime date)
        {
            return await _attendanceRepository.IsDateBookedAsync(userId, date);
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            await _attendanceRepository.AddAttendanceAsync(attendance);
        }

        public async Task<bool> RemoveAttendanceAsync(int userId, DateTime date)
        {
            return await _attendanceRepository.RemoveAttendanceAsync(userId, date);
        }

        public async Task<List<int>> GetUserIdsByDateAsync(DateTime date)
        {
            return await _attendanceRepository.GetUserIdsByDateAsync(date);
        }

        // public async Task<List<DateOnly>> GetAttendanceDatesByUserAsync(int userId)
        // {
        //     return await _context.Attendance
        //         .Where(a => a.UserId == userId)
        //         .Select(a => a.Date)
        //         .ToListAsync();
        // }
    }
}
