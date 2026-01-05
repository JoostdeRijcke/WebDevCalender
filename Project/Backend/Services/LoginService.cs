using CalendifyApp.Models;
using CalendifyApp.Utils;
using CalendifyApp.Repositories.Interfaces;
using System.Linq;

namespace CalendifyApp.Services
{
    public enum LoginStatus { IncorrectPassword, IncorrectUsernameOrEmail, Success }

    public enum ADMIN_SESSION_KEY { adminLoggedIn }

    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;

        public LoginService(IUserRepository userRepository, IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
        }

        public LoginStatus CheckPassword(string email, string inputPassword)
        {
            // Check if the admin exists in the database
            var admin = _adminRepository.GetAdminByEmail(email);
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
            var user = _userRepository.GetUserByEmail(email); // Match email
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
            if (_userRepository.UserExists(user.Email)) return 3;
            if (user.Password.Length < 6) return 2;
            if (!user.Email.Contains("@") || !user.Email.Contains(".")) return 1;
            user.Password = EncryptionHelper.EncryptPassword(user.Password);
            user.Id = _userRepository.GetUserCount() + 1;
            user.restoreCode = null;
            _userRepository.AddUser(user);
            _userRepository.SaveChangesAsync().Wait();
            return 0;
        }


        public int GenerateCode()
        {
            Random random = new Random();
            int randomNumberInRange = random.Next(100000, 999999);
            return randomNumberInRange;
        }

        public bool ChangeCode(int code, string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user != null)
            {
                user.restoreCode = code;
                _userRepository.UpdateUser(user);
                _userRepository.SaveChangesAsync().Wait();
                return true;
            }
            return false;
        }
        public bool CheckCode(int code, string email)
        {
            var user = _userRepository.GetUserByEmailAndRestoreCode(email, code);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public bool Password(string email, string password)
        {

            User user = _userRepository.GetUserByEmail(email);
            if (password.Length < 6) return false;
            if (user != null)
            {
                user.Password = EncryptionHelper.EncryptPassword(password);
                _userRepository.UpdateUser(user);
                _userRepository.SaveChangesAsync().Wait();
                return true;
            }
            return false;
        }

        public User? GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

    }
}