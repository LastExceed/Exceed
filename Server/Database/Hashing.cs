using System;
using System.Security.Cryptography;

namespace Server.Database {
    public static class Hashing {
        public const int SaltSize = 32;
        public const int HashSize = 32;
        private const int Iterations = 10000;
        
        public static byte[] Hash(string password) {
            byte[] salt = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return hashBytes;
        }
        public static bool Verify(string password, byte[] hashedPassword) {
            var salt = new byte[SaltSize];
            Array.Copy(hashedPassword, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            
            for(var i = 0; i < HashSize; i++) {
                if(hashedPassword[i + SaltSize] != hash[i]) {
                    return false;
                }
            }
            return true;
        }
    }
}