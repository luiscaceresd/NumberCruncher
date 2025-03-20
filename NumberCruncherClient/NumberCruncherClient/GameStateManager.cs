using System;
using System.IO;
using System.Text.Json;

namespace NumberCruncherClient
{
    /// <summary>
    /// Manages the saving and loading of the game state using JSON serialization.
    /// </summary>
    public class GameStateManager
    {
        // The file path where the game state is saved.
        private const string filePath = "gamestate.json";

        /// <summary>
        /// Saves the current game state to a JSON file.
        /// </summary>
        /// <param name="game">The NumberCruncherGame instance to save.</param>
        public void saveState(NumberCruncherGame game)
        {
            try
            {
                // Configure JSON serializer to include fields and indent for readability.
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                };
                string json = JsonSerializer.Serialize(game, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error saving game state: " + ex.Message);
            }
        }

        /// <summary>
        /// Loads the game state from the JSON file.
        /// </summary>
        /// <returns>The loaded NumberCruncherGame instance, or null if the file doesn't exist or an error occurs.</returns>
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