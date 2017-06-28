using System;
using System.Security.Cryptography;
using System.Text;

namespace Resources {
    public sealed class Hashing {
        private const int saltSize = 16;
        private const int hashSize = 20;
        
        public static byte[] Hash(string password, int iterations) {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[saltSize]);

            //Create Hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(hashSize);

            var hashInfo = Encoding.UTF8.GetBytes($"$V1${iterations}");
            var hashBytes = new byte[saltSize + hashSize + hashInfo.Length];
            salt.CopyTo(hashBytes, 0);
            hash.CopyTo(hashBytes, saltSize);
            hashInfo.CopyTo(hashBytes, hashSize + saltSize);

            return hashBytes;
        }

        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        public static byte[] Hash(string password) {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Checks if hash is supported
        /// </summary>
        public static bool IsHashSupported(byte[] hash) {
            return Encoding.UTF8.GetString(hash, hashSize + saltSize, hash.Length - (hashSize + saltSize)).Contains("$V1$");
        }

        /// <summary>
        /// verify a password against a hash
        /// </summary>
        public static bool Verify(string password, byte[] hashedPassword) {
            //check hash
            if(!IsHashSupported(hashedPassword)) {
                throw new NotSupportedException("The hashtype is not supported");
            }

            //extract iteration and Base64 string
            var iterations = int.Parse(Encoding.UTF8.GetString(hashedPassword, hashSize + saltSize, hashedPassword.Length - (hashSize + saltSize)).Replace("$V1$", ""));
            var hashBytes = new byte[hashSize + saltSize];
            Array.Copy(hashedPassword, 0, hashBytes, 0, hashSize + saltSize);

            //get salt
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(hashSize);

            //get result
            for(var i = 0; i < hashSize; i++) {
                if(hashBytes[i + saltSize] != hash[i]) {
                    return false;
                }
            }
            return true;
        }
    }
}