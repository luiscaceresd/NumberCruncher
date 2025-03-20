using System;

namespace NumberCruncherClient
{
    /// <summary>
    /// Represents a player in the NumberCruncher game.
    /// Stores the player's initials, score, and the number of levels completed.
    /// </summary>
    [Serializable]
    public class Player
    {
        // The player's initials.
        private string initials = string.Empty;

        // The player's current score.
        private int score;

        // The number of levels the player has completed.
        private int levelsCompleted;

        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        public Player() { }

        /// <summary>
        /// Gets the player's initials.
        /// </summary>
        /// <returns>The player's initials.</returns>
        public string getInitials() => initials;

        /// <summary>
        /// Sets the player's initials.
        /// </summary>
        /// <param name="initials">The initials to set.</param>
        public void setInitials(string initials) => this.initials = initials;

        /// <summary>
        /// Gets the player's current score.
        /// </summary>
        /// <returns>The player's score.</returns>
        public int getScore() => score;

        /// <summary>
        /// Sets the player's score.
        /// </summary>
        /// <param name="score">The score to set.</param>
        public void setScore(int score) => this.score = score;

        /// <summary>
        /// Gets the number of levels the player has completed.
        /// </summary>
        /// <returns>The number of levels completed.</returns>
        public int getLevelsCompleted() => levelsCompleted;

        /// <summary>
        /// Sets the number of levels completed.
        /// </summary>
        /// <param name="levels">The number of levels to set.</param>
        public void setLevelsCompleted(int levels) => this.levelsCompleted = levels;

        /// <summary>
        /// Updates the player's score by adding the specified points.
        /// </summary>
        /// <param name="points">The points to add to the score.</param>
        public void updateScore(int points) => score += points;
    }
}