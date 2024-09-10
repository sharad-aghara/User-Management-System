using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UMS.BL.Helpers
{
    public static class HashedPasswordHelper
    {
        // Generate a random password (for example)
        public static string GenerateRandomPassword()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            var password = randomNumber.ToString();
            return password;
        }

        // password hashing logic
        //public static string HashPassword(string password)
        //{
        //    // Generate a 128-bit salt using a sequence of
        //    // cryptographically strong random bytes.
        //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes

        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password!,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 100000,
        //        numBytesRequested: 256 / 8));

        //    return hashed;
        //}

        // Hash the password with a salt
        public static string HashPassword(string password)
        {
            // Generate a 128-bit salt
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            //using (var rng = new RNGCryptoServiceProvider())
            //{
            //    rng.GetBytes(salt);
            //}

            // Hash the password with the salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Combine salt and hash (typically stored in the same field or separate fields)
            return Convert.ToBase64String(salt) + ":" + hashed;
        }

        // Verify a password against a stored hash
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Extract the salt and hash from the stored hash
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                throw new InvalidOperationException("The stored hash is not in the correct format.");
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            string storedHashedPassword = parts[1];

            // Hash the entered password with the extracted salt
            string enteredHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Compare the hashed entered password with the stored hash
            return enteredHashedPassword == storedHashedPassword;
        }
    }
}
