using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NumberCruncherClient
{
    /// <summary>
    /// Represents the core of the NumberCruncher game.
    /// Manages the player, levels, game state, scoring, and game flow.
    /// </summary>
    [Serializable]
    // we make these properties public with getters and setters so that we can serialize them,
    // the JSON serializer will not see them otherwise
    public class NumberCruncherGame
    {
        // The player object, containing initials, score, and levels completed.
        public Player player { get; set; }

        // Manages the levels and tracks for the game.
        public LevelManager levelManager { get; set; }

        // Handles saving and loading the game state.
        [JsonIgnore] public GameStateManager gameStateManager { get; set; }

        // Calculates the score based on spare guesses.
        [JsonIgnore] public Scorer scorer { get; set; }

        // The selected difficulty level for the game.
        public Difficulty difficulty { get; set; }

        // Tracks spare guesses to carry over to the next level.
        public int spareGuessesForNextLevel { get; set; } = 0;

        public int currentMaxRange { get; set; }

        public int GetCurrentMaxRange() => currentMaxRange;


        /// <summary>
        /// Initializes a new instance of the NumberCruncherGame class.
        /// Sets up all necessary components for the game.
        /// </summary>
        public NumberCruncherGame()
        {
            player = new Player();
            levelManager = new LevelManager();
            gameStateManager = new GameStateManager();
            scorer = new Scorer();
        }

        /// <summary>
        /// Gets the player object.
        /// </summary>
        public Player Player => player;

        /// <summary>
        /// Gets or sets the difficulty level of the game.
        /// </summary>
        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }

        /// <summary>
        /// Gets the number of tracks in the current level.
        /// </summary>
        public Track[] GetTracks() => levelManager.GetTracks();

        /// <summary>
        /// Starts the game by setting up the tracks for the first level.
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


            // Start with no extra attempts for the first level.
            levelManager.SetupTracks(difficulty, 0, currentMaxRange);

        }

        /// <summary>
        /// Processes a complete level based on the player's guesses for each track.
        /// Calculates spare guesses, updates the score, and saves the game state.
        /// </summary>
        /// <param name="guessesPerTrack">An array of guess arrays, one for each track.</param>
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

            // Get active tracks
            Track[] activeTracks = allTracks.Take(activeTrackCount).ToArray();

            if (filteredGuesses.Length != activeTracks.Length)
            {
                throw new ArgumentException($"Number of guess arrays must match the number of active tracks. Expected {activeTracks.Length}, but got {filteredGuesses.Length}.");
            }

            int totalSpareGuesses = 0;

            for (int index = 0; index < activeTracks.Length; index++)
            {
                Track track = activeTracks[index];

                if (filteredGuesses[index] == null || filteredGuesses[index].Length == 0)
                {
                    Console.WriteLine($"Warning: Track {index} has no guesses. Defaulting to max attempts.");
                    continue; // Skip processing this track
                }

                int attemptsUsed = 0;
                bool correct = false;

                foreach (int guess in filteredGuesses[index])
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

            // Check for game completion or winning condition
            if (player.getLevelsCompleted() >= levelManager.GetLevelNumber()) // Assuming this is how you know when the game ends.
            {
                // Handle game over condition, display results, or transition to game over state.
                Console.WriteLine("Congratulations! You've completed the game.");
                return 0;
            }
            foreach (var track in levelManager.GetTracks())
            {
                track.guessHistory.Clear();
            }
            // Save the game state after completing the level.
            gameStateManager.saveState(this);

            return totalSpareGuesses;
        }

        /// <summary>
        /// Advances the game to the next level, setting up new tracks with carried-over spare guesses.
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
            gameStateManager.saveState(this);
        }

        /// <summary>
        /// Loads a saved game state.
        /// </summary>
        /// <returns>The loaded NumberCruncherGame instance, or null if loading fails.</returns>
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