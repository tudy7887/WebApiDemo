using Learning.Model;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace Learning.Services
{
    public class PasswordEncription
    {
        private byte[] salt;
        private byte[] hash;
        private const int keySize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public PasswordEncription(string rawPassword)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            Encryption(rawPassword);
        }
        public PasswordEncription(string rawPassword, byte[] userSalt)
        {
            salt = userSalt;
            Encryption(rawPassword);
        }
        public byte[] GetSalt() { return salt; }
        public byte[] GetHash() { return hash; }
        private void Encryption(string rawPassword)
        {
            hash = Rfc2898DeriveBytes.Pbkdf2(
               Encoding.UTF8.GetBytes(rawPassword),
               salt,
               iterations,
               hashAlgorithm,
               keySize);
        }
    }
}
