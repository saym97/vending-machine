using System.Security.Cryptography;
using System.Text;

namespace coreServices.Helper
{
    public class PasswordHelper
    {
        private static string GenerateSalt()
        {
            byte[] salt;
            RandomNumberGenerator.Create().GetBytes(salt = new byte[16]);
            return Convert.ToBase64String(salt);
        }


        public static void GenerateHash(string password, out string salt, out string hash)
        {
            salt = GenerateSalt();

            byte[] v = Encoding.UTF8.GetBytes(String.Concat(salt, password));
            SHA256 hmacsha256 =  SHA256.Create();
            hash = Convert.ToBase64String(hmacsha256.ComputeHash(v)); 
        }


        public static bool VerifyPasswordHash(string password, string storedSalt, string storedHash)
        {
            byte[] v = Encoding.UTF8.GetBytes(String.Concat(storedSalt, password));
            SHA256 hmacsha256 = SHA256.Create();
            string generatedHash = Convert.ToBase64String(hmacsha256.ComputeHash(v));

            return generatedHash == storedHash;
        }
    }
}
