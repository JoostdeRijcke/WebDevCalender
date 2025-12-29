using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IAdminRepository : IRepository<Admin>
    {
        // Admin-specific query methods
        Task<Admin?> GetByEmailAsync(string email);
        Task<Admin?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
    }
}
