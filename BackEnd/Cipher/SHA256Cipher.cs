using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Security.Cryptography;
using System.Text;

namespace Blazor_OpenBMCLAPI.BackEnd.Cipher
{
    public static class SHA256Cipher
    {
        public static (string hashUserName, string hashPassword, string salt) CreatePasswordHash(string userName, string password)
        {
            byte[] saltBytes = GenerateSalt(16); // 16字节的随机盐
            string salt = Convert.ToBase64String(saltBytes);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] userNameBytes = Encoding.UTF8.GetBytes(userName);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hashPassword = Convert.ToBase64String(hashBytes);
                hashBytes = sha256.ComputeHash(userNameBytes);
                string hashUserName = Convert.ToBase64String(hashBytes);
                return (hashUserName, hashPassword, salt);
            }
        }
        public static string GetUserNameHash(string userName)
        {
            byte[] userNameBytes = Encoding.UTF8.GetBytes(userName);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashbBytes = sha256.ComputeHash(userNameBytes);
                return Convert.ToBase64String(hashbBytes);
            }
        }
        public static bool VerifyPassword(string userName, string password, string verifyHashUserName, string verifyHashPassword, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] userNameBytes = Encoding.UTF8.GetBytes(userName);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hashPassword = Convert.ToBase64String(hashBytes);
                hashBytes = sha256.ComputeHash(userNameBytes);
                string hashUserName = Convert.ToBase64String(hashBytes);
                return (verifyHashUserName == hashUserName) && (verifyHashPassword == hashPassword);
            }
        }

        // Method to generate a random salt
        private static byte[] GenerateSalt(int SaltSize)
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
    }
}
