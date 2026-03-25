using System;
using System.Text;

namespace Program
{
    public static class EncryptionUtitlity
    {
        private static readonly string _allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly string _altChars = "AQ1SW2DE3FR4GT5HYJUKILOPZMXNCBVqa6ws7ed8rf9tgyhujikolpmznxbcv";

        public static string Encrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return password;
            var sb = new StringBuilder();
            foreach (var ch in password)
            {
                var charIndex = _allChars.IndexOf(ch);
                sb.Append(charIndex != -1 ? _altChars[charIndex] : ch);
            }
            return sb.ToString();
        }

        public static string Decrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return password;
            var sb = new StringBuilder();
            foreach (var ch in password)
            {
                var charIndex = _altChars.IndexOf(ch);
                sb.Append(charIndex != -1 ? _allChars[charIndex] : ch);
            }
            return sb.ToString();
        }
    }
}