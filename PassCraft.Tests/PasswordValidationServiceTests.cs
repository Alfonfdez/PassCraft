using PassCraft.Core.Services;

namespace PassCraft.Tests
{
    /// <summary>
    /// Unit test suite protecting character pool state evaluation workflows.
    /// </summary>
    public class PasswordValidationServiceTests
    {
        private readonly PasswordValidationService _validationService;

        public PasswordValidationServiceTests()
        {
            // Initialize the system under test (SUT)
            _validationService = new PasswordValidationService();
        }

        [Fact]
        public void IsPoolValid_AllFiltersDisabled_ReturnsFalse()
        {
            // Arrange & Act
            bool result = _validationService.IsPoolValid(
                useUpper: false,
                useLower: false,
                useNumbers: false,
                useSymbols: false,
                customSymbols: null,
                excludedChars: null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsPoolValid_OnlySymbolsActiveButTextIsEmpty_ReturnsFalse(string? emptySymbols)
        {
            // Arrange & Act
            bool result = _validationService.IsPoolValid(
                useUpper: false,
                useLower: false,
                useNumbers: false,
                useSymbols: true,
                customSymbols: emptySymbols,
                excludedChars: null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPoolValid_AllCharactersAreExcluded_ReturnsFalse()
        {
            // Arrange
            // We enable only numbers, but exclude all possible numbers from '0' to '9'
            bool useNumbers = true;
            string excludedAllDigits = "0123456789";

            // Act
            bool result = _validationService.IsPoolValid(
                useUpper: false,
                useLower: false,
                useNumbers: useNumbers,
                useSymbols: false,
                customSymbols: null,
                excludedChars: excludedAllDigits);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPoolValid_StandardValidConfiguration_ReturnsTrue()
        {
            // Arrange & Act
            bool result = _validationService.IsPoolValid(
                useUpper: true,
                useLower: true,
                useNumbers: false,
                useSymbols: false,
                customSymbols: null,
                excludedChars: "ABC"); // Excluded partial, characters still remain

            // Assert
            Assert.True(result);
        }
    }
}
