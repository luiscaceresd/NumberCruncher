using NumberCruncherClient;
using Xunit;

namespace NumberCruncherClient.Tests
{
    /// <summary>
    /// Tests for the Track class focusing on mode generation, guess validation, and feedback.
    /// </summary>
    public class TrackTests
    {
        /// <summary>
        /// Ensures that generateMode returns a value within the specified range.
        /// </summary>
        [Fact]
        public void GenerateMode_ReturnsValueWithinRange()
        {
            int minValue = 1;
            int maxValue = 10;
            int allowedAttempts = 5;
            Track testTrack = new Track(minValue, maxValue, allowedAttempts);

            int generatedMode = testTrack.generateMode();
            Assert.InRange(generatedMode, minValue, maxValue);
        }

        /// <summary>
        /// Verifies that CheckGuess returns true when the guess equals the target mode.
        /// </summary>
        [Fact]
        public void CheckGuess_ReturnsTrue_ForCorrectGuess()
        {
            int minValue = 1;
            int maxValue = 10;
            int allowedAttempts = 5;
            Track testTrack = new Track(minValue, maxValue, allowedAttempts);
            int modeValue = testTrack.generateMode();
            testTrack.setMode(modeValue);

            Assert.True(testTrack.CheckGuess(modeValue));
        }

        /// <summary>
        /// Verifies that CheckGuess returns false when the guess does not match the target mode.
        /// </summary>
        [Fact]
        public void CheckGuess_ReturnsFalse_ForIncorrectGuess()
        {
            int minValue = 1;
            int maxValue = 10;
            int allowedAttempts = 5;
            Track testTrack = new Track(minValue, maxValue, allowedAttempts);
            int modeValue = testTrack.generateMode();
            testTrack.setMode(modeValue);

            int incorrectGuess = modeValue == maxValue ? modeValue - 1 : modeValue + 1;
            Assert.False(testTrack.CheckGuess(incorrectGuess));
        }

        /// <summary>
        /// Tests that GetFeedback returns the correct directional feedback.
        /// For a fixed mode of 6:
        /// - A guess of 5 returns "↑" (too low),
        /// - A guess of 7 returns "↓" (too high),
        /// - A guess of 6 returns an empty string (correct).
        /// </summary>
        /// <param name="guessValue">The player's guess.</param>
        /// <param name="expectedFeedback">The expected feedback ("↑", "↓", or "").</param>
        [Theory]
        [InlineData(5, "↑")]
        [InlineData(7, "↓")]
        [InlineData(6, "")]
        public void GetFeedback_ReturnsAppropriateFeedback(int guessValue, string expectedFeedback)
        {
            int fixedMode = 6;
            Track testTrack = new Track(1, 10, 5);
            testTrack.setMode(fixedMode);

            string actualFeedback = testTrack.GetFeedback(guessValue);
            Assert.Equal(expectedFeedback, actualFeedback);
        }
    }
}
