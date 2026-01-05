using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;

namespace CalendifyApp.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly MyContext _context;

        public UserRepository(MyContext context)
        {
            _context = context;
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }

        public User? GetUserById(int userId)
        {
            return _context.Users.SingleOrDefault(x => x.Id == userId);
        }

        public bool UserExists(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email) != null;
        }

        public int GetUserCount()
        {
            return _context.Users.Count();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public User? GetUserByEmailAndRestoreCode(string email, int code)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email && x.restoreCode == code);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
