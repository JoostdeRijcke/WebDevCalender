using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithEventAttendancesAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.EventAttendances)
                .ThenInclude(ea => ea.Event)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetUsersWithRecurringDaysAsync(int recurringDays)
        {
            return await _dbSet
                .Where(u => u.RecurringDays == recurringDays)
                .ToListAsync();
        }

        public async Task<int> GetNextUserIdAsync()
        {
            var maxId = await _dbSet.MaxAsync(u => (int?)u.Id) ?? 0;
            return maxId + 1;
        }
    }
}
