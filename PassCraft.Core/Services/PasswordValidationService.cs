using System.Text;
using PassCraft.Core.Contracts;

namespace PassCraft.Core.Services
{
    /// <summary>
    /// Core validation service engine managing real-time character matrix evaluations.
    /// </summary>
    public class PasswordValidationService : IPasswordValidationService
    {
        public bool IsPoolValid(bool useUpper, bool useLower, bool useNumbers, bool useSymbols, string? customSymbols, string? excludedChars)
        {
            // 1. Check if at least one category is active
            if (!useUpper && !useLower && !useNumbers && !useSymbols)
            {
                return false;
            }

            // 2. Calculate the potential pool based on active configurations
            var sb = new StringBuilder();
            if (useUpper) sb.Append(Constants.Constants.CharacterPools.Uppercase);
            if (useLower) sb.Append(Constants.Constants.CharacterPools.Lowercase);
            if (useNumbers) sb.Append(Constants.Constants.CharacterPools.Numbers);

            if (useSymbols && !string.IsNullOrWhiteSpace(customSymbols))
                sb.Append(customSymbols);

            string pool = sb.ToString();

            // 3. Filter out explicitly excluded configurations
            if (!string.IsNullOrEmpty(excludedChars))
            {
                foreach (char c in excludedChars)
                {
                    pool = pool.Replace(c.ToString(), "");
                }
            }

            // 4. Return whether any usable tokens are left
            return pool.Length > 0;
        }

        public bool AreCharacterSetsEquivalent(string? input, string targetReference)
        {
            if (input == null) return targetReference.Length == 0;

            // Using HashSets extracts unique characters and ignores sequence order entirely
            var inputSet = new HashSet<char>(input);
            var targetSet = new HashSet<char>(targetReference);

            return inputSet.SetEquals(targetSet);
        }
    }
}
