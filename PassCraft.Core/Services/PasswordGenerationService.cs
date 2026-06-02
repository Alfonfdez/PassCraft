using PassCraft.Core.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace PassCraft.Core.Services
{
    public class PasswordGenerationService : IPasswordGenerationService
    {
        public string GeneratePassword(int length, bool includeUpper, bool includeLower, bool includeNumbers, string? customSymbols, string? excludedChars)
        {
            if (length <= 0) return string.Empty;

            var charPool = new StringBuilder();
            if (includeUpper) charPool.Append(Constants.Constants.CharacterPools.Uppercase);
            if (includeLower) charPool.Append(Constants.Constants.CharacterPools.Lowercase);
            if (includeNumbers) charPool.Append(Constants.Constants.CharacterPools.Numbers);
            if (!string.IsNullOrEmpty(customSymbols)) charPool.Append(customSymbols);

            string pool = charPool.ToString();

            if (!string.IsNullOrEmpty(excludedChars))
            {
                foreach (char c in excludedChars)
                {
                    pool = pool.Replace(c.ToString(), "");
                }
            }

            if (pool.Length == 0) return string.Empty;

            char[] password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = pool[RandomNumberGenerator.GetInt32(pool.Length)];
            }
            return new string(password);
        }
    }
}
