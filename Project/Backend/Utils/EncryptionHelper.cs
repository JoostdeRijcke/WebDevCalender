using System.Security.Cryptography;
using System.Text;


namespace CalendifyApp.Utils
{
    public static class EncryptionHelper
    {
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(password));

                // Convert to Base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}