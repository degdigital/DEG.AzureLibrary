using System.Text;
using System.Text.RegularExpressions;

namespace DEG.AzureLibrary.Storage
{
    /// <summary>
    /// Class RowKeySanitizer.
    /// </summary>
    public static class KeySanitizer
    {
        static readonly Regex DisallowedCharsInTableKeys = new Regex(@"[\\\\#%+/?\u0000-\u001F\u007F-\u009F]");

        /// <summary>
        /// Sanitizes the key value.
        /// </summary>
        /// <param name="key">The key value.</param>
        /// <param name="disallowedCharReplacement">Character to replace disallowed with.</param>
        /// <param name="prefixWithHash">Prefix value with hash.</param>
        public static string Sanitize(string key, string disallowedCharReplacement, bool prefixWithHash)
        {
            string sanitizedKey = DisallowedCharsInTableKeys.Replace(key, disallowedCharReplacement);

            if (prefixWithHash)
                sanitizedKey = $"{CreateMd5(key)}{disallowedCharReplacement}{sanitizedKey}";

            return sanitizedKey;
        }

        /// <summary>
        /// Creates MD5 hash.
        /// </summary>
        /// <param name="input">The input value.</param>
        static string CreateMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
