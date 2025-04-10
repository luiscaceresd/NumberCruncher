using System;
using System.IO;
using System.Text.Json;

namespace NumberCruncherClient
{
    /// <summary>
    /// Manages saving and loading of the game state via JSON serialization.
    /// The game state is stored in the local Application Data folder.
    /// </summary>
    public class GameStateManager
    {
        // Full file path for saving game state.
        private static readonly string SaveFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "NumberCruncherGame", "gamestate.json"
        );

        /// <summary>
        /// Serializes and writes the current game state to a JSON file.
        /// </summary>
        /// <param name="currentGame">The NumberCruncherGame instance to save.</param>
        public void SaveState(NumberCruncherGame currentGame)
        {
            // Create the directory if it doesn't exist.
            string saveDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "NumberCruncherGame"
            );
            Directory.CreateDirectory(saveDirectory);

            // Build full path with player's initials.
            string path = Path.Combine(saveDirectory, $"gamestate_{currentGame.Player.getInitials()}.json");

            // Serialize the current game using indentation and including all fields.
            string jsonData = JsonSerializer.Serialize(currentGame, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });

            // Write the JSON string to the file.
            File.WriteAllText(path, jsonData);
        }

        /// <summary>
        /// Loads a game state from a JSON file selected via file dialog.
        /// </summary>
        /// <returns>The loaded NumberCruncherGame instance, or null on error.</returns>
        public NumberCruncherGame? LoadState()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Load Game State"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string jsonData = File.ReadAllText(openFileDialog.FileName);
                    var options = new JsonSerializerOptions { IncludeFields = true };
                    return JsonSerializer.Deserialize<NumberCruncherGame>(jsonData, options);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading game: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return null;
        }
    }
}
