using NumberCruncherClient;
using Xunit;

namespace NumberCruncherClient.Tests
{
    /// <summary>
    /// Tests for the Scorer class which calculates level scores based on spare guesses.
    /// </summary>
    public class ScorerTests
    {
        /// <summary>
        /// Verifies that calculateScore returns the expected score for various counts of spare guesses.
        /// </summary>
        /// <param name="spareGuesses">The number of spare guesses.</param>
        /// <param name="expectedScore">The expected calculated score.</param>
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 10)]
        [InlineData(3, 80)]   // (3 * 10) + (1 * 50) = 30 + 50 = 80.
        [InlineData(4, 90)]   // (4 * 10) + (1 * 50) = 40 + 50 = 90 (integer division).
        [InlineData(6, 160)]  // (6 * 10) + (2 * 50) = 60 + 100 = 160.
        public void CalculateScore_ReturnsExpectedScore(int spareGuesses, int expectedScore)
        {
            Scorer scorer = new Scorer();
            int actualScore = scorer.calculateScore(spareGuesses);
            Assert.Equal(expectedScore, actualScore);
        }
    }
}
