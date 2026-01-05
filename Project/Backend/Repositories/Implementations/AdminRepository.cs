using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;

namespace CalendifyApp.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MyContext _context;

        public AdminRepository(MyContext context)
        {
            _context = context;
        }

        public Admin? GetAdminByEmail(string email)
        {
            return _context.Admin.SingleOrDefault(x => x.Email == email);
        }
    }
}
