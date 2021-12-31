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
        public static readonly string GodSalt版本 = "Version20220102";
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
   
    public enum PasswordStrength
    {
        /// <summary>
        /// Blank Password (empty and/or space chars only)
        /// 允許空白密碼
        /// </summary>
        Blank = 0,
        /// <summary>
        /// Either too short (less than 5 chars), one-case letters only or digits only
        /// 密碼長度小於5個字元 或者 具有一個字母字元 或者 具有一個數字字元
        /// </summary>
        VeryWeak = 1,
        /// <summary>
        /// At least 5 characters, one strong condition met (>= 8 chars 
        /// with 1 or more UC letters, LC letters, digits & special chars)
        /// 密碼長度至少5個字元 並且符合這裡其中一個條件
        /// 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號
        /// </summary>
        Weak = 2,
        /// <summary>
        /// At least 5 characters, 
        /// two strong conditions met 
        /// (>= 8 chars 
        /// with 1 or more UC letters, LC letters, digits & special chars)
        /// 密碼長度至少5個字元 並且符合這裡其中兩個個條件
        /// 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號
        /// </summary>
        Medium = 3,
        /// <summary>
        /// At least 8 characters, 
        /// three strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// 密碼長度至少8個字元 並且符合這裡其中三個個條件
        /// 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號
        /// </summary>
        Strong = 4,
        /// <summary>
        /// At least 8 characters, all strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// 密碼長度至少8個字元 並且符合這裡所有條件
        /// 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號
        /// </summary>
        VeryStrong = 5
    }

    public class PasswordOptions
    {
        public int RequiredLength { get; set; }
        public int RequiredUniqueChars { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireDigit { get; set; }
    }

    public static class PasswordCheck
    {

        public static string PasswordHint(PasswordStrength passwordStrength)
        {
            string result = "";
            switch (passwordStrength)
            {
                case PasswordStrength.Blank:
                    result = "允許空白密碼";
                    break;
                case PasswordStrength.VeryWeak:
                    result = "密碼長度小於5個字元 或者 具有一個字母字元 或者 具有一個數字字元"; 
                    break;
                case PasswordStrength.Weak:
                    result = "密碼長度至少5個字元 並且符合這裡其中一個條件 : 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號";
                    break;
                case PasswordStrength.Medium:
                    result = "密碼長度至少5個字元 並且符合這裡其中兩個個條件 : 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號";
                    break;
                case PasswordStrength.Strong:
                    result = "密碼長度至少8個字元 並且符合這裡其中三個個條件 : 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號";
                    break;
                case PasswordStrength.VeryStrong:
                    result = "密碼長度至少8個字元 並且符合這裡所有條件 : 密碼長度大於等於 8 個字元、大寫、小寫、數字、特殊符號"; 
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// Generic method to retrieve password strength: use this for general purpose scenarios, 
        /// i.e. when you don't have a strict policy to follow.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordStrength GetPasswordStrength(string password)
        {
            int score = 0;
            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim())) return PasswordStrength.Blank;
            if (HasMinimumLength(password, 5)) score++;
            if (HasMinimumLength(password, 8)) score++;
            if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password)) score++;
            if (HasDigit(password)) score++;
            if (HasSpecialChar(password)) score++;
            return (PasswordStrength)score;
        }

        /// <summary>
        /// Sample password policy implementation:
        /// - minimum 8 characters
        /// - at lease one UC letter
        /// - at least one LC letter
        /// - at least one non-letter char (digit OR special char)
        /// </summary>
        /// <returns></returns>
        public static bool IsStrongPassword(string password)
        {
            return HasMinimumLength(password, 8)
                && HasUpperCaseLetter(password)
                && HasLowerCaseLetter(password)
                && (HasDigit(password) || HasSpecialChar(password));
        }

        /// <summary>
        /// Sample password policy implementation following the Microsoft.AspNetCore.Identity.PasswordOptions standard.
        /// </summary>
        public static bool IsValidPassword(string password, PasswordOptions opts)
        {
            return IsValidPassword(
                password,
                opts.RequiredLength,
                opts.RequiredUniqueChars,
                opts.RequireNonAlphanumeric,
                opts.RequireLowercase,
                opts.RequireUppercase,
                opts.RequireDigit);
        }


        /// <summary>
        /// Sample password policy implementation following the Microsoft.AspNetCore.Identity.PasswordOptions standard.
        /// </summary>
        public static bool IsValidPassword(
            string password,
            int requiredLength,
            int requiredUniqueChars,
            bool requireNonAlphanumeric,
            bool requireLowercase,
            bool requireUppercase,
            bool requireDigit)
        {
            if (!HasMinimumLength(password, requiredLength)) return false;
            if (!HasMinimumUniqueChars(password, requiredUniqueChars)) return false;
            if (requireNonAlphanumeric && !HasSpecialChar(password)) return false;
            if (requireLowercase && !HasLowerCaseLetter(password)) return false;
            if (requireUppercase && !HasUpperCaseLetter(password)) return false;
            if (requireDigit && !HasDigit(password)) return false;
            return true;
        }

        #region Helper Methods

        public static bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }

        public static bool HasMinimumUniqueChars(string password, int minUniqueChars)
        {
            return password.Distinct().Count() >= minUniqueChars;
        }

        /// <summary>
        /// Returns TRUE if the password has at least one digit
        /// </summary>
        public static bool HasDigit(string password)
        {
            return password.Any(c => char.IsDigit(c));
        }

        /// <summary>
        /// Returns TRUE if the password has at least one special character
        /// </summary>
        public static bool HasSpecialChar(string password)
        {
            // return password.Any(c => char.IsPunctuation(c)) || password.Any(c => char.IsSeparator(c)) || password.Any(c => char.IsSymbol(c));
            return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
        }

        /// <summary>
        /// Returns TRUE if the password has at least one uppercase letter
        /// </summary>
        public static bool HasUpperCaseLetter(string password)
        {
            return password.Any(c => char.IsUpper(c));
        }

        /// <summary>
        /// Returns TRUE if the password has at least one lowercase letter
        /// </summary>
        public static bool HasLowerCaseLetter(string password)
        {
            return password.Any(c => char.IsLower(c));
        }
        #endregion
    }
}
