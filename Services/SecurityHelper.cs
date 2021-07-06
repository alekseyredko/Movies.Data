using System;
using System.Security.Cryptography;

namespace Movies.Data.Services
{
    class SecurityHelper
    {
        private static readonly int iterationsNum = 100;
        private static readonly int saltSize = 16;
        private static readonly int hashSize = 16;

        public static string GenerateSalt()
        {
            var saltBytes = new byte[saltSize];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterationsNum))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(hashSize));
            }
        }

        public static bool IsVerified(string password, string salt, string hash)
        {
            return HashPassword(password, salt) == hash;
        }
    }
}