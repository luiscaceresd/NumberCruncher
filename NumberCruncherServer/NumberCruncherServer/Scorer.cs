using System;

namespace NumberCruncherServer
{
    /// <summary>
    /// Responsible for calculating the score based on spare guesses.
    /// Each spare guess is worth 10 points plus a bonus of 50 points for every 3 spare guesses.
    /// </summary>
    public class Scorer
    {
        /// <summary>
        /// Calculates and returns the score based on spare guesses.
        /// </summary>
        public int calculateScore(int spareGuesses)
        {
            int score = spareGuesses * 10;
            int bonus = (spareGuesses / 3) * 50;
            return score + bonus;
        }
    }
}
