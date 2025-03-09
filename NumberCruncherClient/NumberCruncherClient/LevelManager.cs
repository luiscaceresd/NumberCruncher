using System;

namespace NumberCruncherClient
{
    /// <summary>
    /// Manages game levels and tracks.
    /// Sets up tracks based on the current level and chosen difficulty.
    /// </summary>
    [Serializable]
    public class LevelManager
    {
        private int levelNumber;
        private Track[] tracks;

        /// <summary>
        /// Default constructor.
        /// Initializes level number to 1 and creates an empty track array.
        /// </summary>
        public LevelManager()
        {
            levelNumber = 1;
            // Initialize tracks to an empty array to satisfy non-nullable field requirements.
            tracks = Array.Empty<Track>();
        }

        /// <summary>
        /// Gets the current level number.
        /// </summary>
        public int GetLevelNumber() { return levelNumber; }

        /// <summary>
        /// Sets the current level number.
        /// </summary>
        public void SetLevelNumber(int level) { levelNumber = level; }

        /// <summary>
        /// Gets the tracks for the current level.
        /// </summary>
        public Track[] GetTracks() { return tracks; }

        /// <summary>
        /// Sets the tracks for the current level.
        /// </summary>
        public void SetTracks(Track[] tracks) { this.tracks = tracks; }

        /// <summary>
        /// Increases the level number by one.
        /// </summary>
        public void IncreaseLevel()
        {
            levelNumber++;
        }

        /// <summary>
        /// Sets up the tracks for the current level based on the chosen difficulty.
        /// </summary>
        /// <param name="difficulty">The selected game difficulty.</param>
        public void SetupTracks(Difficulty difficulty)
        {
            int numberOfTracks = 0;
            int baseRange = 0;
            int allowedAttemptsBase = 0;

            // Determine track parameters based on difficulty.
            switch (difficulty)
            {
                case Difficulty.EASY:
                    numberOfTracks = 3;
                    baseRange = 10;     // Range: 1..10
                    allowedAttemptsBase = 5;
                    break;
                case Difficulty.MODERATE:
                    numberOfTracks = 5;
                    baseRange = 100;    // Range: 1..100
                    allowedAttemptsBase = 7;
                    break;
                case Difficulty.DIFFICULT:
                    numberOfTracks = 7;
                    baseRange = 1000;   // Range: 1..1000
                    allowedAttemptsBase = 11;
                    break;
            }

            // Increase the range based on the level.
            int rangeMax = baseRange * (levelNumber == 0 ? 1 : levelNumber);
            tracks = new Track[numberOfTracks];

            // Create and initialize each track.
            for (int index = 0; index < numberOfTracks; index++)
            {
                tracks[index] = new Track(1, rangeMax, allowedAttemptsBase);
                int mode = tracks[index].generateMode();
                tracks[index].setMode(mode);
            }
        }
    }
}
