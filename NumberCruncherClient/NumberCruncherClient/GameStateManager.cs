using System;
using System.IO;
using System.Text.Json;

namespace NumberCruncherClient
{
    /// <summary>
    /// Manages saving and loading the game state.
    /// Uses System.Text.Json for serialization instead of the obsolete BinaryFormatter.
    /// </summary>
    public class GameStateManager
    {
        private const string filePath = "gamestate.json";

        /// <summary>
        /// Saves the current game state to a JSON file.
        /// </summary>
        public void saveState(NumberCruncherGame game)
        {
            try
            {
                // Configure JsonSerializer options.
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                };

                // Serialize the game object to JSON.
                string json = JsonSerializer.Serialize(game, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error saving game state: " + ex.Message);
            }
        }

        /// <summary>
        /// Loads the game state from a JSON file.
        /// Returns null if the file does not exist or an error occurs.
        /// </summary>
        public NumberCruncherGame? loadState()
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };
                return JsonSerializer.Deserialize<NumberCruncherGame>(json, options);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error loading game state: " + ex.Message);
                return null;
            }
        }
    }
}
