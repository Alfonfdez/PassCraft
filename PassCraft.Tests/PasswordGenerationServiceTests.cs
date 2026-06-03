using PassCraft.Core.Services;

namespace PassCraft.Tests
{
    /// <summary>
    /// Unit test suite verifying security boundaries and generation criteria execution.
    /// </summary>
    public class PasswordGenerationServiceTests
    {
        private readonly PasswordGenerationService _generationService;

        public PasswordGenerationServiceTests()
        {
            _generationService = new PasswordGenerationService();
        }

        [Theory]
        [InlineData(5)]
        [InlineData(16)]
        [InlineData(30)]
        public void GeneratePassword_ValidLengthRequested_ReturnsExactLength(int requestedLength)
        {
            // Arrange & Act
            string password = _generationService.GeneratePassword(
                length: requestedLength,
                includeUpper: true,
                includeLower: true,
                includeNumbers: true,
                customSymbols: null,
                excludedChars: null);

            // Assert
            Assert.Equal(requestedLength, password.Length);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void GeneratePassword_InvalidOrZeroLength_ReturnsEmptyString(int invalidLength)
        {
            // Arrange & Act
            string password = _generationService.GeneratePassword(
                length: invalidLength,
                includeUpper: true,
                includeLower: true,
                includeNumbers: true,
                customSymbols: null,
                excludedChars: null);

            // Assert
            Assert.Empty(password);
        }

        [Fact]
        public void GeneratePassword_WithExcludedCharacters_NeverContainsExcludedTokens()
        {
            // Arrange
            int length = 30;
            string excludedList = "ABCDEF123456";

            // Act
            // Run the generation multiple times to ensure statistical reliability 
            for (int i = 0; i < 50; i++)
            {
                string password = _generationService.GeneratePassword(
                    length: length,
                    includeUpper: true,
                    includeLower: true,
                    includeNumbers: true,
                    customSymbols: null,
                    excludedChars: excludedList);

                // Assert
                foreach (char excludedChar in excludedList)
                {
                    Assert.DoesNotContain(excludedChar.ToString(), password);
                }
            }
        }

        [Fact]
        public void GeneratePassword_EmptyCalculatedPool_ReturnsEmptyString()
        {
            // Arrange & Act
            // Scenario A: Everything turned off
            string passwordNoCategories = _generationService.GeneratePassword(
                length: 10,
                includeUpper: false,
                includeLower: false,
                includeNumbers: false,
                customSymbols: "",
                excludedChars: null);

            // Scenario B: Turned on, but completely neutralized by the exclusion rule
            string passwordAllFiltered = _generationService.GeneratePassword(
                length: 10,
                includeUpper: false,
                includeLower: false,
                includeNumbers: true,
                customSymbols: null,
                excludedChars: "0123456789");

            // Assert
            Assert.Empty(passwordNoCategories);
            Assert.Empty(passwordAllFiltered);
        }
    }
}
