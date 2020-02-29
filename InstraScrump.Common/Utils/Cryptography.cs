using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Extension;

namespace InstaScrump.Common.Utils
{
    public static class Cryptography
    {
        private static int _iterations = 2;
        private static int _keySize = 256;

        private static string _hash = "SHA1";

        public static Tuple<string, string> Encrypt<T>(string value, string password, string vector) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = vector.GetBytes<ASCIIEncoding>();
            var salt = KeyGenerator.GetUniqueKey(25);
            var saltBytes = salt.GetBytes<UTF8Encoding>();
            var valueBytes = value.GetBytes<UTF8Encoding>();

            byte[] encrypted;
            using (var cipher = new T())
            {
                var passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                var keyBytes = passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                using (var to = new MemoryStream())
                using (var writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                {
                    writer.Write(valueBytes, 0, valueBytes.Length);
                    writer.FlushFinalBlock();
                    encrypted = to.ToArray();
                }

                cipher.Clear();
            }

            return new Tuple<string, string>(Convert.ToBase64String(encrypted), salt);
        }

        public static string Decrypt<T>(string value, string password, string salt, string vector) where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = vector.GetBytes<ASCIIEncoding>();
            var saltBytes = salt.GetBytes<UTF8Encoding>();
            var valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            var decryptedByteCount = 0;

            using (var cipher = new T())
            {
                var passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                var keyBytes = passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                try
                {
                    using (var decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    using (var from = new MemoryStream(valueBytes))
                    using (var reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                    {
                        decrypted = new byte[valueBytes.Length];
                        decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                    }
                }
                catch (Exception e )
                {
                    return string.Empty;
                }

                cipher.Clear();
            }

            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
    }
}