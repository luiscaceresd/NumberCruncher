using System;

namespace NumberCruncherServer
{
    /// <summary>
    /// Represents a player in the NumberCruncher game.
    /// Contains player initials, score, and number of levels completed.
    /// </summary>
    [Serializable]
    public class Player
    {
        // Private fields initialized to avoid null reference issues.
        private string initials = string.Empty;
        private int score;
        private int levelsCompleted;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Player() { }

        /// <summary>
        /// Gets the player's initials.
        /// </summary>
        public string getInitials() { return initials; }

        /// <summary>
        /// Sets the player's initials.
        /// </summary>
        public void setInitials(string initials) { this.initials = initials; }

        /// <summary>
        /// Gets the player's current score.
        /// </summary>
        public int getScore() { return score; }

        /// <summary>
        /// Sets the player's score.
        /// </summary>
        public void setScore(int score) { this.score = score; }

        /// <summary>
        /// Gets the number of levels the player has completed.
        /// </summary>
        public int getLevelsCompleted() { return levelsCompleted; }

        /// <summary>
        /// Sets the number of levels completed.
        /// </summary>
        public void setLevelsCompleted(int levels) { this.levelsCompleted = levels; }

        /// <summary>
        /// Updates the player's score by adding the given points.
        /// </summary>
        public void updateScore(int points)
        {
            score += points;
        }
    }
}
