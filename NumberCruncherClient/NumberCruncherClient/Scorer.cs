namespace NumberCruncherClient
{
    /// <summary>
    /// Calculates the player's score based on spare guesses.
    /// </summary>
    public class Scorer
    {
        /// <summary>
        /// Calculates the score for a level.
        /// Each spare guess adds 10 points, and every 3 spare guesses yield an additional bonus of 50 points.
        /// </summary>
        /// <param name="spareGuesses">Number of spare guesses earned in the level.</param>
        /// <returns>The total score for the level.</returns>
        public int calculateScore(int spareGuesses)
        {
            int baseScore = spareGuesses * 10;
            int bonusScore = (spareGuesses / 3) * 50;
            return baseScore + bonusScore;
        }
    }
}
