using CalendifyApp.Models;
using CalendifyApp.Utils;
using System.Linq;

namespace CalendifyApp.Services
{
    public enum LoginStatus { IncorrectPassword, IncorrectUsernameOrEmail, Success }

    public enum ADMIN_SESSION_KEY { adminLoggedIn }

    public class LoginService : ILoginService
    {
        private readonly MyContext _context;

        public LoginService(MyContext context)
        {
            _context = context;
        }

        public LoginStatus CheckPassword(string email, string inputPassword)
        {
            // Check if the admin exists in the database
            var admin = _context.Admin.SingleOrDefault(x => x.Email == email);
            if (admin != null)
            {
                // Validate the password
                if (admin.Password == EncryptionHelper.EncryptPassword(inputPassword))
                {
                    return LoginStatus.Success;
                }
                return LoginStatus.IncorrectPassword;
            }

            return LoginStatus.IncorrectUsernameOrEmail;
        }

        public LoginStatus CheckUserPassword(string email, string inputPassword)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email); // Match email
            if (user != null)
            {
                if (user.Password == EncryptionHelper.EncryptPassword(inputPassword)) // Compare plain text
                {
                    return LoginStatus.Success;
                }
                return LoginStatus.IncorrectPassword;
            }
            return LoginStatus.IncorrectUsernameOrEmail;
        }

        public int Register(User user)
        {
            if (_context.Users.SingleOrDefault(x => x.Email == user.Email) != null) return 3;
            if (user.Password.Length < 6) return 2;
            if (!user.Email.Contains("@") || !user.Email.Contains(".")) return 1;
            user.Password = EncryptionHelper.EncryptPassword(user.Password);
            user.Id = _context.Users.Count() + 1;
            _context.Users.Add(user);
            _context.SaveChanges();
            return 0;
        }


        public int ForgotPassword()
        {
            Random random = new Random();
            int randomNumberInRange = random.Next(100000, 999999);
            return randomNumberInRange;
        }

        public bool Password(string email, string password)
        {

            User user = _context.Users.SingleOrDefault(x => x.Email == email);
            if (user != null)
            {
                user.Password = EncryptionHelper.EncryptPassword(password);
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}