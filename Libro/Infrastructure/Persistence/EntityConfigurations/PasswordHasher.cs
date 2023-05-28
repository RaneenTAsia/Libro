using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public static class PasswordHasher
    {
        public static string ComputeHash(string password, string salt, int iteration)
        {
            if (iteration <= 0) return password;

            using var sha256 = SHA256.Create();
            var passwordSalt = $"{password}{salt}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSalt);
            var byteHash = sha256.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);

            return ComputeHash(hash, salt, iteration - 1);
        }

        public static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);

            return salt;
        }

        public static bool VerifyPassword(string password, string salt,int iteration, string passwordToCompare)
        {
            string generatedPassword = ComputeHash(password, salt, iteration);
            return generatedPassword.Equals(passwordToCompare);
        }
    }
}
