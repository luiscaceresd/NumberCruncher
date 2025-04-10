using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text.Json.Serialization;


namespace NumberCruncherClient
{
    /// <summary>
    /// Represents a single track in the NumberCruncher game.
    /// Each track generates a set of random numbers and calculates a unique mode (the number to guess).
    /// Provides methods to check guesses and give feedback.
    /// </summary>
    [Serializable]
    // making these properties public for serialization to work
    public class Track
    {

        // Array to hold the generated random numbers.
        [JsonIgnore] private int[] randomNumbers { get; set; }

        // The mode of the random numbers, which is the target for the player to guess.
        public int mode { get; set;}

        // The number of attempts the player is allowed for this track.
        public int allowedAttempts { get; set; }

        // Minimum value for the random number range.
        public int rangeMin { get; set; }

        // Maximum value for the random number range.
        public int rangeMax { get; set; }

        // Shared random instance to generate numbers consistently.
        private static Random random = new Random();

        // List to store the player's guess history
        [JsonIgnore] public List<int> guessHistory { get; set; } = new List<int>(); 

        /// <summary>
        /// Initializes a new instance of the Track class.
        /// </summary>
        /// <param name="rangeMin">The minimum value for the random numbers.</param>
        /// <param name="rangeMax">The maximum value for the random numbers.</param>
        /// <param name="allowedAttempts">The number of attempts allowed for this track.</param>
        public Track(int rangeMin, int rangeMax, int allowedAttempts)
        {
            this.rangeMin = rangeMin;
            this.rangeMax = rangeMax;
            this.allowedAttempts = allowedAttempts;
            // Start with an empty array; numbers will be generated later.
            randomNumbers = Array.Empty<int>();
        }

        /// <summary>
        /// Gets the array of random numbers generated for this track.
        /// </summary>
        /// <returns>The array of random numbers.</returns>
        public int[] GetRandomNumbers() => randomNumbers;

        /// <summary>
        /// Sets the array of random numbers for this track.
        /// </summary>
        /// <param name="nums">The array of numbers to set.</param>
        public void SetRandomNumbers(int[] nums) => randomNumbers = nums;

        /// <summary>
        /// Gets the mode (the number to guess) for this track.
        /// </summary>
        /// <returns>The mode of the random numbers.</returns>
        public int GetMode() => mode;

        /// <summary>
        /// Sets the mode for this track.
        /// </summary>
        /// <param name="mode">The mode to set.</param>
        public void setMode(int mode) => this.mode = mode;

        /// <summary>
        /// Gets the number of allowed attempts for this track.
        /// </summary>
        /// <returns>The allowed attempts.</returns>
        public int GetAllowedAttempts() => allowedAttempts;

        /// <summary>
        /// Sets the number of allowed attempts for this track.
        /// </summary>
        /// <param name="attempts">The number of attempts to set.</param>
        public void SetAllowedAttempts(int attempts) => allowedAttempts = attempts;

        /// <summary>
        /// Generates a set of random numbers and calculates a unique mode.
        /// If multiple modes are found, the process repeats until a unique mode is determined.
        /// </summary>
        /// <returns>The unique mode for this track.</returns>
        public int generateMode()
        {
            bool uniqueModeFound = false;
            int computedMode = 0;

            // Loop until a unique mode is found.
            while (!uniqueModeFound)
            {
                randomNumbers = new int[1000];
                // Generate 1000 random numbers within the specified range.
                for (int i = 0; i < 1000; i++)
                {
                    randomNumbers[i] = random.Next(rangeMin, rangeMax + 1);
                }

                // Count the frequency of each number using a dictionary.
                Dictionary<int, int> frequency = new Dictionary<int, int>();
                foreach (int num in randomNumbers)
                {
                    if (frequency.ContainsKey(num))
                        frequency[num]++;
                    else
                        frequency[num] = 1;
                }

                // Find the maximum frequency.
                int maxFrequency = frequency.Values.Max();

                // Get all numbers that have the maximum frequency.
                var modes = frequency.Where(pair => pair.Value == maxFrequency)
                                     .Select(pair => pair.Key)
                                     .ToList();

                // If there is exactly one mode, accept it.
                if (modes.Count == 1)
                {
                    uniqueModeFound = true;
                    computedMode = modes[0];
                }
                // If multiple modes are found, the loop will repeat to generate a new set.
            }
            return computedMode;
        }

        /// <summary>
        /// Checks if the player's guess matches the mode.
        /// </summary>
        /// <param name="guess">The player's guess.</param>
        /// <returns>True if the guess is correct, false otherwise.</returns>
        public bool CheckGuess(int guess) => guess == mode;



        

        /// <summary>
        /// Provides feedback on the player's guess.
        /// </summary>
        /// <param name="guess">The player's guess.</param>
        /// <returns>"↑" if the guess is too low, "↓" if too high, "✔" if correct.</returns>
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