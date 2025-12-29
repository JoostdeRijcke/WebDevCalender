using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Repositories.Implementations
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(MyContext context) : base(context)
        {
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Admin?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(a => a.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet
                .AnyAsync(a => a.Username == username);
        }
    }
}
