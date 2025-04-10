using NumberCruncherClient;
using Xunit;

namespace NumberCruncherClient.Tests
{
    /// <summary>
    /// Tests for LevelManager which sets up tracks based on difficulty and extra attempts.
    /// </summary>
    public class LevelManagerTests
    {
        /// <summary>
        /// Verifies that SetupTracks creates the correct number of tracks with proper allowedAttempts.
        /// </summary>
        /// <param name="difficulty">The game difficulty.</param>
        /// <param name="maxRange">The maximum number for the guessing range.</param>
        /// <param name="extraAttempts">Extra attempts added to base attempts.</param>
        /// <param name="expectedTrackCount">Expected number of tracks for the difficulty.</param>
        /// <param name="baseAttempts">Base allowed attempts per track for the difficulty.</param>
        [Theory]
        [InlineData(Difficulty.EASY, 10, 0, 3, 5)]
        [InlineData(Difficulty.MODERATE, 100, 0, 5, 7)]
        [InlineData(Difficulty.DIFFICULT, 1000, 0, 7, 11)]
        public void SetupTracks_ShouldInitializeTracksCorrectly(Difficulty difficulty, int maxRange, int extraAttempts, int expectedTrackCount, int baseAttempts)
        {
            // Create a new LevelManager instance.
            LevelManager levelManager = new LevelManager();
            levelManager.SetupTracks(difficulty, extraAttempts, maxRange);
            Track[] tracks = levelManager.GetTracks();

            // Verify number of tracks.
            Assert.Equal(expectedTrackCount, tracks.Length);
            foreach (var track in tracks)
            {
                // Allowed attempts should equal base attempts plus extra attempts.
                Assert.Equal(baseAttempts + extraAttempts, track.allowedAttempts);
                // Mode should be generated within the range.
                Assert.InRange(track.GetMode(), 1, maxRange);
            }
        }
    }
}
