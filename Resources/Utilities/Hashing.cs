using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Resources {
    public sealed class Hashing {
        //DO NOT CHANGE CONSTANTS
        public const int saltSize = 16;
        public const int hashSize = 20; 
        private const int itterations = 10000; 

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

            return $"$CUBEHASH$V1${itterations}${base64Hash}";
        }

        public static byte[] Hash(string password, byte[] salt) {
            return new Rfc2898DeriveBytes(password, salt, itterations).GetBytes(hashSize);
        }

        /// <summary>
        /// Checks if hash is supported
        /// </summary>
        public static bool IsHashSupported(string hash) {
            return hash.Contains("$CUBEHASH$V1$");
        }

        /// <summary>
        /// verify a password against a hash
        /// </summary>
        public static bool Verify(string password, string hashedPassword) {
            if(!IsHashSupported(hashedPassword)) {
                throw new NotSupportedException("The hashtype is not supported");
            }

            var splits = hashedPassword.Split('$');
            var iterations = int.Parse(splits[3]);
            var hashBytes = Convert.FromBase64String(splits[4]);
            
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            var hash = new byte[hashSize];
            Array.Copy(hashBytes, saltSize, hash, 0, hashSize);
            
            byte[] reverse = new Rfc2898DeriveBytes(password, salt, iterations).GetBytes(hashSize);
            
            return hash.SequenceEqual(reverse);
        }

        public static Tuple<string, string> CreateKeyPair() {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(1024, cspParams);

            string publicKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(false));
            string privateKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(true));

            return new Tuple<string, string>(privateKey, publicKey);
        }

        public static byte[] Encrypt(string publicKey, string data) {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));

            byte[] plainBytes = Encoding.UTF8.GetBytes(data);

            return rsaProvider.Encrypt(plainBytes, false);
        }
        public static byte[] Encrypt(string publicKey, byte[] data) {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));
            
            return rsaProvider.Encrypt(data, false);
        }

        public static string Decrypt(string privateKey, byte[] encryptedBytes) {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(privateKey));

            byte[] plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

            string plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);

            return plainText;
        }
        public static byte[] DecryptB(string privateKey, byte[] encryptedBytes) {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(privateKey));

            return rsaProvider.Decrypt(encryptedBytes, false);
        }
    }
}