using System;

namespace NumberCruncherClient
{
    /// <summary>
    /// Represents the core of the NumberCruncher game.
    /// Manages the player, levels, game state, scoring, and game flow.
    /// </summary>
    [Serializable]
    public class NumberCruncherGame
    {
        // The player object, containing initials, score, and levels completed.
        private Player player;

        // Manages the levels and tracks for the game.
        private LevelManager levelManager;

        // Handles saving and loading the game state.
        private GameStateManager gameStateManager;

        // Calculates the score based on spare guesses.
        private Scorer scorer;

        // The selected difficulty level for the game.
        private Difficulty difficulty;

        // Tracks spare guesses to carry over to the next level.
        private int spareGuessesForNextLevel = 0;

        private int currentMaxRange;

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
            int totalSpareGuesses = 0;
            Track[] tracks = levelManager.GetTracks();

            if (guessesPerTrack.Length != tracks.Length)
            {
                throw new ArgumentException("Number of guess arrays must match the number of tracks.");
            }

            for (int index = 0; index < tracks.Length; index++)
            {
                Track track = tracks[index];
                int attemptsUsed = 0;
                bool correct = false;

                foreach (int guess in guessesPerTrack[index])
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
            GameStateManager gsm = new GameStateManager();
            return gsm.loadState() ?? null;
        }

        
    }
}