using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByEmail(string email);
        User? GetUserById(int userId);
        bool UserExists(string email);
        int GetUserCount();
        void AddUser(User user);
        void UpdateUser(User user);
        User? GetUserByEmailAndRestoreCode(string email, int code);
        Task SaveChangesAsync();
    }
}
