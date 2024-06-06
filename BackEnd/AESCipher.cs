using System.Security.Cryptography;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class AESCipher
    {
        private static readonly int SaltSize = 16; // Size in bytes
        private static readonly int KeySize = 32;  // Size in bytes for AES-256
        private static readonly int Iterations = 100000; // Recommended iterations

        // Method to generate a random salt
        private static byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        // Method to derive a key and IV from a password and salt
        private static void DeriveKeyAndIV(string password, byte[] salt, out byte[] key, out byte[] iv)
        {
            using (var keyGenerator = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                key = keyGenerator.GetBytes(KeySize);
                iv = keyGenerator.GetBytes(16); // AES block size is 16 bytes
            }
        }

        // Method to encrypt a string
        public static string Encrypt(string plainText, string password)
        {
            byte[] salt = GenerateSalt();
            DeriveKeyAndIV(password, salt, out byte[] key, out byte[] iv);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(salt, 0, salt.Length); // Prepend salt to the output
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        // Method to decrypt a string
        public static string Decrypt(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] salt = new byte[SaltSize];
            Array.Copy(cipherBytes, 0, salt, 0, salt.Length);

            DeriveKeyAndIV(password, salt, out byte[] key, out byte[] iv);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(cipherBytes, SaltSize, cipherBytes.Length - SaltSize))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }

}
