using System;
using System.Security.Cryptography;
using System.Text;

namespace MeetingRoomManagement.Services
{
    public class PasswordHelper
    {
        public static string HashPasswordSHA1(string password)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
         
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            string inputHash = HashPasswordSHA1(inputPassword);
            return string.Equals(inputHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
