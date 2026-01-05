using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly MyContext _context;

        public AttendanceRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<bool> IsDateBookedAsync(int userId, DateTime date)
        {
            return await _context.Attendance.AnyAsync(a => a.UserId == userId && a.Date == date);
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            await _context.Attendance.AddAsync(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveAttendanceAsync(int userId, DateTime date)
        {
            var existingAttendance = await _context.Attendance
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Date == date);

            if (existingAttendance != null)
            {
                _context.Attendance.Remove(existingAttendance);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<int>> GetUserIdsByDateAsync(DateTime date)
        {
            return await _context.Attendance
                .Where(a => a.Date == date)
                .Select(a => a.UserId)
                .ToListAsync();
        }
    }
}
