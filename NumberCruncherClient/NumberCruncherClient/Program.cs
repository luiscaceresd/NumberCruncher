using System.IO;

namespace NumberCruncherClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Check if a saved game exists.
            if (File.Exists("gamestate.json"))
            {
                // Prompt the player to load the saved game.
                if (PromptLoadGame())
                {
                    NumberCruncherGame loadedGame = NumberCruncherGame.LoadGame();
                    if (loadedGame != null)
                    {
                        RunGame(loadedGame);
                        return;
                    }
                }
            }

            // If no saved game or player chooses not to load, start a new game.
            using (SplashScreen splash = new SplashScreen())
            {
                splash.ShowDialog();
            }
            Application.Run(new PlayerSetupForm());
        }

        /// <summary>
        /// Prompts the player to decide whether to load the saved game.
        /// </summary>
        /// <returns>True if the player wants to load the saved game, false otherwise.</returns>
        private static bool PromptLoadGame()
        {
            // This method should be implemented to ask the player via UI.
            return true; // Placeholder
        }

        /// <summary>
        /// Runs the game with the loaded game state.
        /// </summary>
        /// <param name="game">The loaded NumberCruncherGame instance.</param>
        private static void RunGame(NumberCruncherGame game)
        {
            // Set up the UI with the loaded game state.
        }
    }
}