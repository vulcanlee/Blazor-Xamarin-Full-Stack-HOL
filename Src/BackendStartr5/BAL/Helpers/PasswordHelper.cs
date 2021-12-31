using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Helpers
{
    public static class PasswordHelper
    {
        public static readonly string GodSalt版本 = "Version20211110";
        public static string GetPasswordSHA(string salt, string password)
        {
            string assemblyPassword = $"{password}-{salt}@";
            SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(assemblyPassword));
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < bytes.Length; j++)
            {
                builder.Append(bytes[j].ToString("x2"));
            }
            password = builder.ToString();
            return password;
        }
        public static string GetGodPasswordSHA(string salt, string password)
        {
            string assemblyPassword = $"{password}-{salt}@Vulcan-Backend{PasswordHelper.GodSalt版本}";
            SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(assemblyPassword));
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < bytes.Length; j++)
            {
                builder.Append(bytes[j].ToString("x2"));
            }
            password = builder.ToString();
            return password;
        }
    }
}
