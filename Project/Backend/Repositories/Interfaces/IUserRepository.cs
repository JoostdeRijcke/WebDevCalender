using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // User-specific query methods
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetUserWithEventAttendancesAsync(int userId);
        Task<IEnumerable<User>> GetUsersWithRecurringDaysAsync(int recurringDays);
        Task<int> GetNextUserIdAsync();
    }
}
