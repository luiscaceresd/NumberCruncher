using System;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace NumberCruncherClient
{
    /// <summary>
    /// Represents the core logic of the NumberCruncher game.
    /// Manages player information, level progression, scoring, and overall game flow.
    /// </summary>
    [Serializable]
    public class NumberCruncherGame
    {
        // Player details (initials, score, levels completed).
        public Player player { get; set; }
        // Level management.
        public LevelManager levelManager { get; set; }
        // Game state manager for saving/loading.
        [JsonIgnore] public GameStateManager gameStateManager { get; set; }
        // Scorer calculates the score based on spare guesses.
        [JsonIgnore] public Scorer scorer { get; set; }
        // The selected difficulty level.
        public Difficulty difficulty { get; set; }
        // Spare guesses carried to the next level.
        public int spareGuessesForNextLevel { get; set; } = 0;
        // Current maximum range for guesses.
        public int currentMaxRange { get; set; }

        /// <summary>
        /// Returns the current maximum guessing range.
        /// </summary>
        public int GetCurrentMaxRange() => currentMaxRange;

        /// <summary>
        /// Initializes a new game instance and sets up essential components.
        /// </summary>
        public NumberCruncherGame()
        {
            player = new Player();
            levelManager = new LevelManager();
            gameStateManager = new GameStateManager();
            scorer = new Scorer();
        }

        /// <summary>
        /// Gets the Player object.
        /// </summary>
        public Player Player => player;

        /// <summary>
        /// Gets or sets the game difficulty.
        /// </summary>
        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }

        /// <summary>
        /// Returns the array of tracks for the current level.
        /// </summary>
        public Track[] GetTracks() => levelManager.GetTracks();

        /// <summary>
        /// Starts a new game by initializing the maximum range and level 1 tracks.
        /// </summary>
        public void startGame()
        {
            currentMaxRange = difficulty switch
            {
                Difficulty.EASY => 10,
                Difficulty.MODERATE => 100,
                Difficulty.DIFFICULT => 1000,
                _ => 10
            };

            // Set up tracks for level 1 with no extra attempts.
            levelManager.SetupTracks(difficulty, 0, currentMaxRange);
        }

        /// <summary>
        /// Processes a level by evaluating the player's guesses, updating score and clearing guess history.
        /// </summary>
        /// <param name="guessesPerTrack">Array of guess arrays per track.</param>
        /// <returns>Total spare guesses for the level.</returns>
        public int ProcessLevel(int[][] guessesPerTrack)
        {
            if (guessesPerTrack == null)
            {
                throw new ArgumentNullException(nameof(guessesPerTrack), "Guesses array cannot be null.");
            }

            Track[] allTracks = levelManager.GetTracks();
            int activeTrackCount = difficulty switch
            {
                Difficulty.EASY => 3,
                Difficulty.MODERATE => 5,
                Difficulty.DIFFICULT => 7,
                _ => 3
            };

            int[][] filteredGuesses = guessesPerTrack.Take(activeTrackCount).ToArray();
            Track[] activeTracks = allTracks.Take(activeTrackCount).ToArray();

            if (filteredGuesses.Length != activeTracks.Length)
            {
                throw new ArgumentException($"Expected {activeTracks.Length} guess arrays, but got {filteredGuesses.Length}.");
            }

            int totalSpareGuesses = 0;
            for (int i = 0; i < activeTracks.Length; i++)
            {
                Track track = activeTracks[i];
                if (filteredGuesses[i] == null || filteredGuesses[i].Length == 0)
                {
                    Console.WriteLine($"Warning: Track {i} has no guesses; defaulting to max attempts.");
                    continue;
                }

                int attemptsUsed = 0;
                bool correct = false;
                foreach (int guess in filteredGuesses[i])
                {
                    attemptsUsed++;
                    if (track.CheckGuess(guess))
                    {
                        correct = true;
                        break;
                    }
                    if (attemptsUsed >= track.GetAllowedAttempts())
                    {
                        break;
                    }
                }
                if (correct)
                {
                    int spare = track.GetAllowedAttempts() - attemptsUsed;
                    totalSpareGuesses += spare;
                }
            }

            int levelScore = scorer.calculateScore(totalSpareGuesses);
            player.updateScore(levelScore);
            player.setLevelsCompleted(player.getLevelsCompleted() + 1);
            spareGuessesForNextLevel = totalSpareGuesses;

            // Clear guess history for each track.
            foreach (var track in levelManager.GetTracks())
            {
                track.guessHistory.Clear();
            }
            // REMOVED: Auto-save is no longer performed.
            return totalSpareGuesses;
        }

        /// <summary>
        /// Advances the game to the next level by increasing the level number and recalculating the range.
        /// </summary>
        public void nextLevel()
        {
            levelManager.IncreaseLevel();
            switch (difficulty)
            {
                case Difficulty.EASY:
                    currentMaxRange += 10;
                    if (currentMaxRange >= 100)
                    {
                        difficulty = Difficulty.MODERATE;
                        currentMaxRange = 100;
                    }
                    break;
                case Difficulty.MODERATE:
                    currentMaxRange += 100;
                    if (currentMaxRange >= 1000)
                    {
                        difficulty = Difficulty.DIFFICULT;
                        currentMaxRange = 1000;
                    }
                    break;
                case Difficulty.DIFFICULT:
                    currentMaxRange += 1000;
                    break;
            }
            levelManager.SetupTracks(difficulty, spareGuessesForNextLevel, currentMaxRange);
            // REMOVED: Auto-save (SaveGame() call) so that saving is only manual.
        }

        /// <summary>
        /// Saves the current game state.
        /// (This method is only called when the user presses the Save button.)
        /// </summary>
        public void SaveGame()
        {
            gameStateManager.SaveState(this);
        }

        /// <summary>
        /// Loads a saved game state using the provided player initials.
        /// </summary>
        /// <param name="playerInitials">The player's initials from the save file.</param>
        /// <returns>The loaded game instance, or null if not found.</returns>
        public static NumberCruncherGame? LoadGame(string playerInitials)
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "NumberCruncherGame",
                $"gamestate_{playerInitials}.json"
            );
            if (!File.Exists(path)) return null;
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<NumberCruncherGame>(json);
        }

        /// <summary>
        /// Loads a saved game state via a file dialog.
        /// </summary>
        /// <returns>The loaded game instance, or null if loading fails.</returns>
        public static NumberCruncherGame? LoadGameWithDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Load Game State"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    return JsonConvert.DeserializeObject<NumberCruncherGame>(json);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
