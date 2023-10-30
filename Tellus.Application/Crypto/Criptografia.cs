using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Tellus.Application.Crypto
{
    public class Criptografia
    {
        private readonly IConfiguration _configuration;
        public Criptografia(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static string Encrypt(string valor)
        {
            return BCrypt.Net.BCrypt.HashPassword(valor, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        public static bool Verify(string valor, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(valor, hash);
        }
        public string EncryptRsa(string valor)
        {
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(_configuration.GetSection("Cripto:public").Value);
            rsa.FromXmlString(_configuration.GetSection("Cripto:private").Value);
            byte[] encryptedPassword = rsa.Encrypt(Encoding.UTF8.GetBytes(valor), true);
            return Convert.ToBase64String(encryptedPassword);
        }
        public string DecryptRsa(string valor)
        {
            using RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(_configuration.GetSection("Cripto:public").Value);
            rsa.FromXmlString(_configuration.GetSection("Cripto:private").Value);
            byte[] encryptedPassword = Convert.FromBase64String(valor);
            byte[] decryptedPassword = rsa.Decrypt(encryptedPassword, true);
          return Encoding.UTF8.GetString(decryptedPassword);
        }
    }
}
