using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(MyContext context) : base(context)
        {
        }

        public async Task<bool> IsDateBookedAsync(int userId, DateTime date)
        {
            return await _dbSet
                .AnyAsync(a => a.UserId == userId && a.Date == date);
        }

        public async Task<Attendance?> GetAttendanceByUserAndDateAsync(int userId, DateTime date)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Date == date);
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByDateAsync(DateTime date)
        {
            return await _dbSet
                .Where(a => a.Date == date)
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUserIdsByDateAsync(DateTime date)
        {
            return await _dbSet
                .Where(a => a.Date == date)
                .Select(a => a.UserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<DateTime>> GetAttendanceDatesByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .Select(a => a.Date)
                .OrderBy(d => d)
                .ToListAsync();
        }
    }
}
