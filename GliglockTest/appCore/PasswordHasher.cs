using GliglockTest.DbLogic;
using System.Security.Cryptography;
using System.Text;

namespace GliglockTest.appCore
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var sha1 = SHA1.Create();
            var hashedBytes = sha1.ComputeHash(bytes);
            return Encoding.UTF8.GetString(hashedBytes);
        }

        public static bool IsCorrectPassword(User user, string password)
        {
            var passwordHash = HashPassword(password + user.Salt.ToString());
            return passwordHash == user.PasswordHash;
        }
    }
}
