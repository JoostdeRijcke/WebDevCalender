using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using CalendifyApp.Utils;

namespace CalendifyApp.Services
{
    public enum LoginStatus { IncorrectPassword, IncorrectUsernameOrEmail, Success }

    public enum ADMIN_SESSION_KEY { adminLoggedIn }

    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public LoginStatus CheckPassword(string email, string inputPassword)
        {
            // Check if the admin exists in the database
            var admin = _unitOfWork.Admins.GetByEmailAsync(email).Result;
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
            var user = _unitOfWork.Users.GetByEmailAsync(email).Result; // Match email
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
            if (_unitOfWork.Users.EmailExistsAsync(user.Email).Result) return 3;
            if (user.Password.Length < 6) return 2;
            if (!user.Email.Contains("@") || !user.Email.Contains(".")) return 1;
            user.Password = EncryptionHelper.EncryptPassword(user.Password);
            user.Id = _unitOfWork.Users.GetNextUserIdAsync().Result;
            _unitOfWork.Users.AddAsync(user).Wait();
            _unitOfWork.SaveChangesAsync().Wait();
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

            User user = _unitOfWork.Users.GetByEmailAsync(email).Result;
            if (user != null)
            {
                user.Password = EncryptionHelper.EncryptPassword(password);
                _unitOfWork.Users.UpdateAsync(user).Wait();
                _unitOfWork.SaveChangesAsync().Wait();
                return true;
            }
            return false;
        }

    }
}