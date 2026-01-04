using CalendifyApp.Models;

namespace CalendifyApp.Services;

public interface ILoginService
{
    public LoginStatus CheckPassword(string username, string inputPassword);

    public LoginStatus CheckUserPassword(string username, string inputPassword);

    public int Register(User user);
    public int GenerateCode();
    public bool CheckCode(int code, string email);
    public bool ChangeCode(int code, string email);
    public bool Password(string email, string password);
    public User? GetUserByEmail(string email);
}