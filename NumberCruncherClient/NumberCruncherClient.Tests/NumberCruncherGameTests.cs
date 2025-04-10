using System.IO;
using NumberCruncherClient;
using Xunit;

namespace NumberCruncherClient.Tests
{
    /// <summary>
    /// Tests for the core game logic in NumberCruncherGame.
    /// Validates game start, level processing, progression, and persistence.
    /// </summary>
    public class NumberCruncherGameTests : IDisposable
    {
        // Save file path used for testing.
        private readonly string _testSaveFilePath;

        public NumberCruncherGameTests()
        {
            string saveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NumberCruncherGame");
            _testSaveFilePath = Path.Combine(saveDir, "gamestate_UNIT.json");
        }

        /// <summary>
        /// Validates that starting a game with EASY difficulty sets the correct maximum range and number of tracks.
        /// </summary>
        [Fact]
        public void StartGame_ShouldSetCurrentMaxRange_Easy()
        {
            NumberCruncherGame game = new NumberCruncherGame();
            game.Difficulty = Difficulty.EASY;
            game.startGame();

            Assert.Equal(10, game.GetCurrentMaxRange());
            Assert.Equal(3, game.GetTracks().Length);
        }

        /// <summary>
        /// Ensures that ProcessLevel updates the score and player's level correctly when every track's guess is correct on first try.
        /// </summary>
        [Fact]
        public void ProcessLevel_ShouldUpdateScoreAndLevel()
        {
            NumberCruncherGame game = new NumberCruncherGame();
            game.Difficulty = Difficulty.EASY;
            game.startGame();
            Track[] tracks = game.GetTracks();

            // Simulate correct guesses (one per track).
            int[][] guesses = new int[tracks.Length][];
            for (int i = 0; i < tracks.Length; i++)
            {
                guesses[i] = new int[] { tracks[i].GetMode() };
            }

            int spareGuesses = game.ProcessLevel(guesses);
            // For EASY, if each track starts with 5 attempts and one guess used, spare = 4 per track.
            Assert.Equal(4 * tracks.Length, spareGuesses);

            int expectedScore = spareGuesses * 10 + (spareGuesses / 3) * 50;
            Assert.Equal(expectedScore, game.Player.getScore());
        }

        /// <summary>
        /// Checks that nextLevel correctly increases the level and the guessing range.
        /// </summary>
        [Fact]
        public void NextLevel_ShouldIncreaseLevelAndRange()
        {
            NumberCruncherGame game = new NumberCruncherGame();
            game.Difficulty = Difficulty.EASY;
            game.startGame();
            int initialRange = game.GetCurrentMaxRange();
            int initialLevel = game.levelManager.GetLevelNumber();

            // Simulate level completion.
            Track[] tracks = game.GetTracks();
            int[][] guesses = new int[tracks.Length][];
            for (int i = 0; i < tracks.Length; i++)
            {
                guesses[i] = new int[] { tracks[i].GetMode() };
            }
            game.ProcessLevel(guesses);
            game.nextLevel();

            Assert.True(game.levelManager.GetLevelNumber() > initialLevel);
            Assert.True(game.GetCurrentMaxRange() > initialRange);
        }

        /// <summary>
        /// Tests that saving and loading a game preserves the player's initials.
        /// </summary>
        [Fact]
        public void SaveGame_LoadGame_ShouldPersistGameState()
        {
            NumberCruncherGame game = new NumberCruncherGame();
            game.Difficulty = Difficulty.EASY;
            game.startGame();
            game.Player.setInitials("UNIT");

            // Save the game.
            game.SaveGame();

            // Load the game using the proper overload.
            NumberCruncherGame loadedGame = NumberCruncherGame.LoadGame("UNIT");
            Assert.NotNull(loadedGame);
            Assert.Equal("UNIT", loadedGame.Player.getInitials());
        }

        /// <summary>
        /// Cleans up test files after tests run.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(_testSaveFilePath))
                File.Delete(_testSaveFilePath);
        }
    }
}
