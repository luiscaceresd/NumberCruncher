using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NumberCruncherClient
{
    /// <summary>
    /// Represents a single track in the NumberCruncher game.
    /// Each track generates a set of random numbers and calculates a unique mode (the number to guess).
    /// Provides methods to check guesses and give feedback.
    /// </summary>
    [Serializable]
    public class Track
    {
        // Array to hold the generated random numbers.
        [JsonIgnore]
        private int[] randomNumbers { get; set; }

        // The mode of the random numbers, which is the target for the player to guess.
        public int mode { get; set; }

        // The number of attempts the player is allowed for this track.
        public int allowedAttempts { get; set; }

        // Minimum value for the random number range.
        public int rangeMin { get; set; }

        // Maximum value for the random number range.
        public int rangeMax { get; set; }

        // Shared random instance to generate numbers consistently.
        private static Random random = new Random();

        // Removed [JsonIgnore] so that guessHistory is serialized.
        public List<int> guessHistory { get; set; } = new List<int>();

        /// <summary>
        /// Initializes a new instance of the Track class.
        /// </summary>
        public Track(int rangeMin, int rangeMax, int allowedAttempts)
        {
            this.rangeMin = rangeMin;
            this.rangeMax = rangeMax;
            this.allowedAttempts = allowedAttempts;
            randomNumbers = Array.Empty<int>();
        }

        /// <summary>
        /// Gets the array of random numbers generated for this track.
        /// </summary>
        public int[] GetRandomNumbers() => randomNumbers;

        /// <summary>
        /// Sets the array of random numbers for this track.
        /// </summary>
        public void SetRandomNumbers(int[] nums) => randomNumbers = nums;

        /// <summary>
        /// Gets the mode (the number to guess) for this track.
        /// </summary>
        public int GetMode() => mode;

        /// <summary>
        /// Sets the mode for this track.
        /// </summary>
        public void setMode(int mode) => this.mode = mode;

        /// <summary>
        /// Gets the number of allowed attempts for this track.
        /// </summary>
        public int GetAllowedAttempts() => allowedAttempts;

        /// <summary>
        /// Sets the number of allowed attempts for this track.
        /// </summary>
        public void SetAllowedAttempts(int attempts) => allowedAttempts = attempts;

        /// <summary>
        /// Generates a set of random numbers and calculates a unique mode.
        /// </summary>
        public int generateMode()
        {
            bool uniqueModeFound = false;
            int computedMode = 0;

            while (!uniqueModeFound)
            {
                randomNumbers = new int[1000];
                for (int i = 0; i < 1000; i++)
                {
                    randomNumbers[i] = random.Next(rangeMin, rangeMax + 1);
                }

                Dictionary<int, int> frequency = new Dictionary<int, int>();
                foreach (int num in randomNumbers)
                {
                    if (frequency.ContainsKey(num))
                        frequency[num]++;
                    else
                        frequency[num] = 1;
                }

                int maxFrequency = frequency.Values.Max();

                var modes = frequency.Where(pair => pair.Value == maxFrequency)
                                     .Select(pair => pair.Key)
                                     .ToList();

                if (modes.Count == 1)
                {
                    uniqueModeFound = true;
                    computedMode = modes[0];
                }
            }
            return computedMode;
        }

        /// <summary>
        /// Checks if the player's guess matches the mode.
        /// </summary>
        public bool CheckGuess(int guess) => guess == mode;

        /// <summary>
        /// Provides feedback on the player's guess.
        /// </summary>
        public string GetFeedback(int guess)
        {
            if (guess < mode)
                return "↑";  // Too low
            else if (guess > mode)
                return "↓";  // Too high
            else
                return "";  // Correct
        }
    }
}
