using System;

namespace NumberCruncherClient
{
    /// <summary>
    /// Manages the levels and associated tracks in the NumberCruncher game.
    /// Sets up tracks according to the current level, difficulty and extra attempts.
    /// </summary>
    [Serializable]
    public class LevelManager
    {
        // The current level number.
        public int LevelNumber { get; set; }

        // Array holding the Track objects for the current level.
        public Track[] Tracks { get; set; }

        /// <summary>
        /// Initializes a new level manager starting from level 1 with no tracks.
        /// </summary>
        public LevelManager()
        {
            LevelNumber = 1;
            Tracks = Array.Empty<Track>();
        }

        /// <summary>
        /// Returns the current level number.
        /// </summary>
        public int GetLevelNumber() => LevelNumber;

        /// <summary>
        /// Sets the current level number.
        /// </summary>
        /// <param name="level">New level number.</param>
        public void SetLevelNumber(int level) => LevelNumber = level;

        /// <summary>
        /// Returns the tracks for the current level.
        /// </summary>
        public Track[] GetTracks() => Tracks;

        /// <summary>
        /// Assigns the array of tracks for the current level.
        /// </summary>
        /// <param name="tracks">The new array of tracks.</param>
        public void SetTracks(Track[] tracks) => Tracks = tracks;

        /// <summary>
        /// Increments the level number by one.
        /// </summary>
        public void IncreaseLevel() => LevelNumber++;

        /// <summary>
        /// Sets up the tracks based on the selected difficulty, number of extra attempts,
        /// and current maximum range.
        /// </summary>
        /// <param name="selectedDifficulty">The current game difficulty.</param>
        /// <param name="extraAttempts">Extra attempts (spare guesses) to add per track.</param>
        /// <param name="currentMaxRange">The upper limit of the guessing range.</param>
        public void SetupTracks(Difficulty selectedDifficulty, int extraAttempts, int currentMaxRange)
        {
            int numberOfTracks = 0;
            int baseAllowedAttempts = 0;

            // Set base track count and allowed attempts from difficulty.
            switch (selectedDifficulty)
            {
                case Difficulty.EASY:
                    numberOfTracks = 3;
                    baseAllowedAttempts = 5;
                    break;
                case Difficulty.MODERATE:
                    numberOfTracks = 5;
                    baseAllowedAttempts = 7;
                    break;
                case Difficulty.DIFFICULT:
                    numberOfTracks = 7;
                    baseAllowedAttempts = 11;
                    break;
            }

            // Calculate allowed attempts including bonus attempts.
            int allowedAttempts = baseAllowedAttempts + extraAttempts;
            Tracks = new Track[numberOfTracks];

            // Create and initialize each track.
            for (int index = 0; index < numberOfTracks; index++)
            {
                Tracks[index] = new Track(1, currentMaxRange, allowedAttempts);
                int generatedMode = Tracks[index].generateMode();
                Tracks[index].setMode(generatedMode);
            }
        }
    }
}
