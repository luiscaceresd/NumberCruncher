namespace NumberCruncherClient
{
    /// <summary>
    /// Calculates the score based on the number of spare guesses.
    /// </summary>
    public class Scorer
    {
        /// <summary>
        /// Calculates the score for the level.
        /// Each spare guess is worth 10 points, with a bonus of 50 points for every 3 spare guesses.
        /// </summary>
        /// <param name="spareGuesses">The number of spare guesses.</param>
        /// <returns>The calculated score.</returns>
        public int calculateScore(int spareGuesses)
        {
            int score = spareGuesses * 10;
            int bonus = (spareGuesses / 3) * 50;
            return score + bonus;
        }
    }
}