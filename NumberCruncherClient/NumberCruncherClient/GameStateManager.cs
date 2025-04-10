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
        // Store game state in a safe location (AppData)
        private static readonly string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "NumberCruncherGame", "gamestate.json"
        );

        /// <summary>
        /// Saves the current game state to a JSON file.
        /// </summary>
        /// <param name="game">The NumberCruncherGame instance to save.</param>
        public void saveState(NumberCruncherGame game)
        {
            string directory  = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "NumberCruncherGame"
                );
            Directory.CreateDirectory(directory);

            string path = Path.Combine(directory, $"gamestate_{game.player.getInitials()}.json");
            string json = JsonSerializer.Serialize(game, new JsonSerializerOptions
            { 
                WriteIndented = true,
                IncludeFields = true
            });

            File.WriteAllText(path, json);
        }


        



        /// <summary>
        /// Loads the game state from the JSON file.
        /// </summary>
        /// <returns>The loaded NumberCruncherGame instance, or null if the file doesn't exist or an error occurs.</returns>
        public NumberCruncherGame? loadState()
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
                    string json = File.ReadAllText(openFileDialog.FileName);
                    var options = new JsonSerializerOptions { IncludeFields = true };
                    return JsonSerializer.Deserialize<NumberCruncherGame>(json, options);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading game: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return null;
        }
    }
}