using System;
using System.Security.Cryptography;

namespace Server.Database {
    public static class Hashing {
        private const int SaltSize = 32;
        private const int HashSize = 32;
        private const int Iterations = 10000;
        
        public static string Hash(string password) {
            byte[] salt = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }
        public static bool Verify(string password, string hashedPassword) {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            
            for(var i = 0; i < HashSize; i++) {
                if(hashBytes[i + SaltSize] != hash[i]) {
                    return false;
                }
            }
            return true;
        }
    }
}