namespace PassCraft.Core.Contracts
{
    /// <summary>
    /// Defines validation workflows to evaluate password criteria pools prior to generation.
    /// </summary>
    public interface IPasswordValidationService
    {
        /// <summary>
        /// Validates whether a usable pool of characters remains given the current toggle configurations and exclusion limits.
        /// </summary>
        /// <returns>True if the final computed character pool contains at least one valid item; otherwise, false.</returns>
        bool IsPoolValid(bool useUpper, bool useLower, bool useNumbers, bool useSymbols, string? customSymbols, string? excludedChars);

        /// <summary>
        /// Compares an input string against a target reference set of characters, ignoring sequence order and character duplicates.
        /// </summary>
        /// <returns>True if the set of unique characters matches exactly; otherwise, false.</returns>
        bool AreCharacterSetsEquivalent(string? input, string targetReference);
    }
}
