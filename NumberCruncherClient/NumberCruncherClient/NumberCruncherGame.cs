using System;

namespace NumberCruncherClient
{
    /// <summary>
    /// Core class representing the NumberCruncher game.
    /// Manages the player, levels, game state, and scoring.
    /// </summary>
    [Serializable]
    public class NumberCruncherGame
    {
        private Player player;
        private LevelManager levelManager;
        private GameStateManager gameStateManager;
        private Scorer scorer;
        private Difficulty difficulty;

        /// <summary>
        /// Default constructor that initializes all core components.
        /// </summary>
        public NumberCruncherGame()
        {
            player = new Player();
            levelManager = new LevelManager();
            gameStateManager = new GameStateManager();
            scorer = new Scorer();
        }

        /// <summary>
        /// Provides access to the Player object.
        /// The client should set player details (like initials) using this property.
        /// </summary>
        public Player Player => player;

        /// <summary>
        /// Gets or sets the game difficulty.
        /// The client should set this before starting the game.
        /// </summary>
        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }

        /// <summary>
        /// Gets the number of tracks in the game.
        /// This is derived from the LevelManager's tracks.
        /// </summary>
        public int TrackCount => levelManager.GetTracks().Length;

        /// <summary>
        /// Initializes the game state for a new game.
        /// The client should set player data and difficulty before calling this.
        /// </summary>
        public void startGame()
        {
            levelManager.SetupTracks(difficulty);
        }

        /// <summary>
        /// Processes a complete level.
        /// The client passes a two-dimensional array (one array per track) of guesses.
        /// Returns the total number of spare guesses accumulated in the level,
        /// and updates the player's score and levels completed.
        /// </summary>
        /// <param name="guessesPerTrack">
        /// An array where each element is an array of guesses corresponding to a track.
        /// </param>
        /// <returns>Total spare guesses for the level.</returns>
        public int ProcessLevel(int[][] guessesPerTrack)
        {
            int totalSpareGuesses = 0;
            Track[] tracks = levelManager.GetTracks();

            // Ensure the number of provided guess arrays matches the number of tracks.
            if (guessesPerTrack.Length != tracks.Length)
            {
                throw new ArgumentException("Number of guess arrays must match the number of tracks.");
            }

            // Process guesses for each track.
            for (int index = 0; index < tracks.Length; index++)
            {
                Track track = tracks[index];
                int attemptsUsed = 0;
                bool correct = false;

                // Process each guess until either the correct guess is found or attempts run out.
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

                // If guessed correctly, calculate spare guesses.
                if (correct)
                {
                    int spare = track.GetAllowedAttempts() - attemptsUsed;
                    totalSpareGuesses += spare;
                }
            }

            // Calculate score for the level and update the player's state.
            int levelScore = scorer.calculateScore(totalSpareGuesses);
            player.updateScore(levelScore);
            player.setLevelsCompleted(player.getLevelsCompleted() + 1);

            return totalSpareGuesses;
        }

        /// <summary>
        /// Advances the game to the next level.
        /// </summary>
        public void nextLevel()
        {
            levelManager.IncreaseLevel();
            levelManager.SetupTracks(difficulty);
        }
    }
}
