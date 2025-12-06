using System.Security.Cryptography;
using System.Text;

namespace Project.Application.UseCases.Users
{
    internal static class UserPasswordHasher
    {
        public static string Hash(string password)
        {
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(hashedBytes);
        }
    }
}
