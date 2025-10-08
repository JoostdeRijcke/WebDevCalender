using CalendifyApp.Models;

namespace CalendifyApp.Services;

public interface ILoginService
{
    public LoginStatus CheckPassword(string username, string inputPassword);

    public LoginStatus CheckUserPassword(string username, string inputPassword);

    public int Register(User user);
    public int ForgotPassword();
    public bool Password(string email, string password);
}