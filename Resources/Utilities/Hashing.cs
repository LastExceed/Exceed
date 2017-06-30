using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Resources {
    public sealed class Hashing {
        private const int saltSize = 16;
        private const int hashSize = 20;
        private const int itterations = 10000; // DO NOT CHANGE


        public static string Hash(string password) {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[saltSize]);

            //Create Hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, itterations);
            var hash = pbkdf2.GetBytes(hashSize);
            
            var hashBytes = new byte[saltSize + hashSize];
            salt.CopyTo(hashBytes, 0);
            hash.CopyTo(hashBytes, saltSize);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return $"$V1${base64Hash}";
        }

        public static byte[] Hash(string password, byte[] salt) {
            return new Rfc2898DeriveBytes(password, salt, itterations).GetBytes(hashSize);
        }

        /// <summary>
        /// Checks if hash is supported
        /// </summary>
        public static bool IsHashSupported(string hash) {
            return hash.Contains("$V1$");
        }

        /// <summary>
        /// verify a password against a hash
        /// </summary>
        public static bool Verify(string password, string hashedPassword) {
            if(!IsHashSupported(hashedPassword)) {
                throw new NotSupportedException("The hashtype is not supported");
            }

            var splits = hashedPassword.Split('$');
            var iterations = int.Parse(splits[2]);
            var hashBytes = Convert.FromBase64String(splits[3]);
            
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            var hash = new byte[hashSize];
            Array.Copy(hashBytes, saltSize, hash, 0, hashSize);
            
            byte[] reverse = new Rfc2898DeriveBytes(password, salt, iterations).GetBytes(hashSize);
            
            return hash.SequenceEqual(reverse);
        }
    }
}