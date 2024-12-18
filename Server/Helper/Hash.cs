using System.Security.Cryptography;
using System.Text;

namespace Server.Helper
{
    public class Hash
    {
        private const string _salt = "";
        public static string PasswordSHA256(string password)
        {
            password += _salt;
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
