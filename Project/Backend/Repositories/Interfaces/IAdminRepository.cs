using CalendifyApp.Models;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Admin? GetAdminByEmail(string email);
    }
}
