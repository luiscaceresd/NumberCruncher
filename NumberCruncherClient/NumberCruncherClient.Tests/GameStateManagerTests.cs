using System;
using System.IO;
using NumberCruncherClient;
using Xunit;

namespace NumberCruncherClient.Tests
{
    /// <summary>
    /// Tests for the GameStateManager class responsible for saving and loading the game state.
    /// </summary>
    public class GameStateManagerTests : IDisposable
    {
        // Full path of the file used during testing.
        private readonly string _testFilePath;
        private readonly GameStateManager _gameStateManager;
        private readonly NumberCruncherGame _testGame;

        public GameStateManagerTests()
        {
            _gameStateManager = new GameStateManager();
            // Create a test game with a unique player initials.
            _testGame = new NumberCruncherGame();
            _testGame.Player.setInitials("TEST");
            _testGame.Difficulty = Difficulty.EASY;
            _testGame.startGame();

            // Build the expected file path for the test save file.
            string saveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NumberCruncherGame");
            _testFilePath = Path.Combine(saveDir, "gamestate_TEST.json");
        }

        /// <summary>
        /// Verifies that saving the game state creates a file.
        /// </summary>
        [Fact]
        public void SaveState_CreatesFile()
        {
            // Remove the file if it already exists.
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            _gameStateManager.SaveState(_testGame);

            // The file should exist after saving.
            Assert.True(File.Exists(_testFilePath));
        }

        /// <summary>
        /// Verifies that the saved file contains the game state data.
        /// </summary>
        [Fact]
        public void SaveState_FileContainsGameState()
        {
            // Remove the file if it already exists.
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            _gameStateManager.SaveState(_testGame);
            string jsonData = File.ReadAllText(_testFilePath);

            // Check that the file content contains the player initials.
            Assert.Contains("TEST", jsonData);
        }

        /// <summary>
        /// Disposes of the test file after each test.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }
    }
}
