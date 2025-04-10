using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NumberCruncherClient
{
    /// <summary>
    /// Represents the core logic of the NumberCruncher game.
    /// Manages player information, levels, game state, scoring, and overall game flow.
    /// </summary>
    [Serializable]
    public class NumberCruncherGame
    {
        // Player details (initials, score, levels completed).
        public Player player { get; set; }
        // Level management.
        public LevelManager levelManager { get; set; }
        // Used to save and load the game state.
        [JsonIgnore] public GameStateManager gameStateManager { get; set; }
        // Calculates score based on spare guesses.
        [JsonIgnore] public Scorer scorer { get; set; }
        // Selected difficulty.
        public Difficulty difficulty { get; set; }
        // Spare guesses carried over to the next level.
        public int spareGuessesForNextLevel { get; set; } = 0;
        // Current maximum range for guesses.
        public int currentMaxRange { get; set; }

        /// <summary>
        /// Returns the current maximum number of the guessing range.
        /// </summary>
        public int GetCurrentMaxRange() => currentMaxRange;

        /// <summary>
        /// Initializes a new game instance and sets up game components.
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
        /// Gets or sets the current game difficulty.
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
        /// Starts a new game by setting up the initial level and range.
        /// </summary>
        public void startGame()
        {
            currentMaxRange = difficulty switch
            {
                Difficulty.EASY => 10,        // Easy: range 1-10
                Difficulty.MODERATE => 100,    // Moderate: range 1-100
                Difficulty.DIFFICULT => 1000,   // Difficult: range 1-1000
                _ => 10
            };
            // Setup level 1 tracks without spare guesses.
            levelManager.SetupTracks(difficulty, 0, currentMaxRange);
        }

        /// <summary>
        /// Processes level completion by analyzing guesses, updating score,
        /// and saving the game state.
        /// </summary>
        /// <param name="guessesPerTrack">Array of guesses per track.</param>
        /// <returns>The total spare guesses for the level.</returns>
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
                throw new ArgumentException($"Mismatch: expected {activeTracks.Length} guess arrays, but got {filteredGuesses.Length}.");
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

            // Clear each track's guess history for the next level.
            foreach (var track in levelManager.GetTracks())
            {
                track.guessHistory.Clear();
            }
            // Save the game state at the end of the level.
            gameStateManager.SaveState(this);
            return totalSpareGuesses;
        }

        /// <summary>
        /// Advances the game to the next level, recalculating range and setting up tracks.
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
            SaveGame();
        }

        /// <summary>
        /// Saves the current game state.
        /// </summary>
        public void SaveGame()
        {
            gameStateManager.SaveState(this);
        }

        /// <summary>
        /// Loads a saved game state from a predefined file path.
        /// </summary>
        /// <returns>The loaded game instance, or null if not found.</returns>
        public static NumberCruncherGame? LoadGame()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NumberCruncherGame",
                "gamestate.json"
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
