using System;

namespace NumberCruncherClient
{
    /// <summary>
    /// Manages the levels and tracks in the NumberCruncher game.
    /// Sets up tracks based on the current level and difficulty, including extra attempts from spare guesses.
    /// </summary>
    [Serializable]
    public class LevelManager
    {
        // The current level number.
        private int levelNumber;

        // The array of tracks for the current level.
        private Track[] tracks;

        /// <summary>
        /// Initializes a new instance of the LevelManager class.
        /// Starts at level 1 with no tracks.
        /// </summary>
        public LevelManager()
        {
            levelNumber = 1;
            tracks = Array.Empty<Track>();
        }

        /// <summary>
        /// Gets the current level number.
        /// </summary>
        /// <returns>The current level number.</returns>
        public int GetLevelNumber() => levelNumber;

        /// <summary>
        /// Sets the current level number.
        /// </summary>
        /// <param name="level">The level number to set.</param>
        public void SetLevelNumber(int level) => levelNumber = level;

        /// <summary>
        /// Gets the tracks for the current level.
        /// </summary>
        /// <returns>The array of tracks.</returns>
        public Track[] GetTracks() => tracks;

        /// <summary>
        /// Sets the tracks for the current level.
        /// </summary>
        /// <param name="tracks">The array of tracks to set.</param>
        public void SetTracks(Track[] tracks) => this.tracks = tracks;

        /// <summary>
        /// Increases the level number by one.
        /// </summary>
        public void IncreaseLevel() => levelNumber++;

        /// <summary>
        /// Sets up the tracks for the current level based on the difficulty and extra attempts.
        /// </summary>
        /// <param name="difficulty">The selected difficulty level.</param>
        /// <param name="extraAttempts">The number of extra attempts to add to each track.</param>
        public void SetupTracks(Difficulty difficulty, int extraAttempts, int currentMaxRange)
        {
            int numberOfTracks = 0;
            int allowedAttemptsBase = 0;

            // Determine base parameters based on difficulty.
            switch (difficulty)
            {
                case Difficulty.EASY:
                    numberOfTracks = 3;
                    allowedAttemptsBase = 5;
                    break;
                case Difficulty.MODERATE:
                    numberOfTracks = 5;
                    allowedAttemptsBase = 7;
                    break;
                case Difficulty.DIFFICULT:
                    numberOfTracks = 7;
                    allowedAttemptsBase = 11;
                    break;
            }
      
            // Calculate the maximum range for the current level.

            // Calculate allowed attempts, including extra attempts from spare guesses.
            int allowedAttempts = allowedAttemptsBase + extraAttempts;
            tracks = new Track[numberOfTracks];

            // Create and initialize each track.
            for (int index = 0; index < numberOfTracks; index++)
            {
                tracks[index] = new Track(1, currentMaxRange, allowedAttempts);
                int mode = tracks[index].generateMode();
                tracks[index].setMode(mode);
            }
        }
    }
}