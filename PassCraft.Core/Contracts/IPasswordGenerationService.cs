namespace PassCraft.Core.Contracts
{
    /// <summary>
    /// Defines the cryptographic business rules for generating highly secure random passwords.
    /// </summary>
    public interface IPasswordGenerationService
    {
        /// <summary>
        /// Generates a cryptographically strong random password based on custom criteria rules.
        /// </summary>
        /// <param name="length">The character length of the target password.</param>
        /// <param name="includeUpper">True to include uppercase pool values.</param>
        /// <param name="includeLower">True to include lowercase pool values.</param>
        /// <param name="includeNumbers">True to include numeric pool values.</param>
        /// <param name="customSymbols">A specific string of symbols to include. Passes null or empty to ignore.</param>
        /// <returns>A string containing the randomly assembled password characters.</returns>
        string GeneratePassword(int length, bool includeUpper, bool includeLower, bool includeNumbers, string? customSymbols);
    }
}
