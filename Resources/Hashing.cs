using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Resources {
    public static class Hashing {
        private const int saltLength = 16;
        private const int hashLength = 20;
        private const int iterations = 5000;

        public static string Hash(string password) {
            byte[] salt;
            //new RNGCryptoServiceProvider().GetBytes(salt = new byte[saltLength]);
            salt = new byte[saltLength] { 162, 206, 60, 96, 201, 169, 149, 231, 47, 88, 28, 250, 166, 254, 27, 213 };
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, hashLength);
            return Convert.ToBase64String(hashBytes);
        }

        //public static bool Verify(string password, string savedPasswordHash) {
        //    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
        //    byte[] salt = new byte[saltLength];
        //    Array.Copy(hashBytes, 0, salt, 0, saltLength);
        //    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        //    byte[] hash = pbkdf2.GetBytes(hashLength);
        //    for (int i = 0; i < hashLength; i++)
        //        if (hashBytes[i + saltLength] != hash[i])
        //            return false;
        //    return true;
        //}
    }
}
