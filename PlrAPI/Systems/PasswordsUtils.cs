using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PlrAPI.Systems
{
    public class PasswordsUtils
    {
        public static string CreateHashedPass(string password, byte[] salt)
        {
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return hash;
        }

        public static byte[] CreateSalt()
        {
            byte[] salt = new byte[AuthOptions.SaltSize];
            var rndGenerator = RandomNumberGenerator.Create();
            rndGenerator.GetBytes(salt);

            return salt;
        }
    }
}
