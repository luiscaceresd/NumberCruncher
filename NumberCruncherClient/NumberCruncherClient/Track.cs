using System;
using System.Collections.Generic;
using System.Linq;

namespace NumberCruncherClient
{
    /// <summary>
    /// Represents a single track in the game.
    /// A track generates random numbers and calculates a unique mode.
    /// </summary>
    [Serializable]
    public class Track
    {
        private int[] randomNumbers;
        private int mode;
        private int allowedAttempts;
        private int rangeMin;
        private int rangeMax;

        // Shared random instance for generating numbers.
        private static Random random = new Random();

        /// <summary>
        /// Initializes a new instance of Track with the specified range and allowed attempts.
        /// </summary>
        public Track(int rangeMin, int rangeMax, int allowedAttempts)
        {
            this.rangeMin = rangeMin;
            this.rangeMax = rangeMax;
            this.allowedAttempts = allowedAttempts;
            // Initialize randomNumbers to an empty array.
            randomNumbers = Array.Empty<int>();
        }

        /// <summary>
        /// Gets the array of random numbers.
        /// </summary>
        public int[] GetRandomNumbers() { return randomNumbers; }

        /// <summary>
        /// Sets the array of random numbers.
        /// </summary>
        public void SetRandomNumbers(int[] nums) { randomNumbers = nums; }

        /// <summary>
        /// Gets the mode (target number) for the track.
        /// </summary>
        public int GetMode() { return mode; }

        /// <summary>
        /// Sets the mode (target number) for the track.
        /// </summary>
        public void setMode(int mode) { this.mode = mode; }

        /// <summary>
        /// Gets the number of allowed attempts for this track.
        /// </summary>
        public int GetAllowedAttempts() { return allowedAttempts; }

        /// <summary>
        /// Sets the allowed attempts for this track.
        /// </summary>
        public void SetAllowedAttempts(int attempts) { allowedAttempts = attempts; }

        /// <summary>
        /// Generates random numbers and computes a unique mode.
        /// Continues generating until a unique mode is found.
        /// </summary>
        /// <returns>The unique mode for this track.</returns>
        public int generateMode()
        {
            bool uniqueModeFound = false;
            int computedMode = 0;

            while (!uniqueModeFound)
            {
                randomNumbers = new int[1000];
                // Generate 1000 random numbers within the specified range.
                for (int i = 0; i < 1000; i++)
                {
                    randomNumbers[i] = random.Next(rangeMin, rangeMax + 1);
                }

                // Count the frequency of each number.
                Dictionary<int, int> frequency = new Dictionary<int, int>();
                foreach (int num in randomNumbers)
                {
                    if (frequency.ContainsKey(num))
                        frequency[num]++;
                    else
                        frequency[num] = 1;
                }

                // Find the highest frequency.
                int maxFrequency = frequency.Values.Max();

                // Get all numbers that occur with the maximum frequency.
                var modes = frequency.Where(pair => pair.Value == maxFrequency)
                                     .Select(pair => pair.Key)
                                     .ToList();

                // Accept only a unique mode.
                if (modes.Count == 1)
                {
                    uniqueModeFound = true;
                    computedMode = modes[0];
                }
            }
            return computedMode;
        }



        /// <summary>
        /// Checks if the given guess matches the track's mode.
        /// </summary>
        public bool CheckGuess(int guess)
        {
            return guess == mode;
        }

        /// <summary>
        /// Provides feedback based on the player's guess.
        /// Returns an upward arrow if the guess is too low,
        /// a downward arrow if too high, and a checkmark if correct.
        /// </summary>
        public string GetFeedback(int guess)
        {
            if (guess < mode)
                return "↑";  // Too low.
            else if (guess > mode)
                return "↓";  // Too high.
            else
                return "✔";  // Correct.
        }
    }
}
