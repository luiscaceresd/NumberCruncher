using System;

namespace NumberCruncherServer
{
    /// <summary>
    /// Default Program class with a Main method for testing the server-side logic.
    /// In production, the client project should reference the server code and manage user interactions.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new game instance.
            NumberCruncherGame game = new NumberCruncherGame();

            // Set up player details and difficulty.
            // In a real client, these values would be provided by the user.
            game.Player.setInitials("ABC");
            game.Difficulty = Difficulty.MODERATE; // MODERATE creates 5 tracks.

            // Start the game by setting up the initial level.
            game.startGame();

            // For testing, we need to provide dummy guesses for each track.
            // Since MODERATE difficulty creates 5 tracks, our dummy array must have 5 inner arrays.
            int trackCount = 5;  // Adjust this if you change the difficulty.
            int[][] dummyGuesses = new int[trackCount][];

            // Populate each inner array with a dummy guess.
            // For this test, we simply use a guess of 0 for every track.
            for (int i = 0; i < trackCount; i++)
            {
                dummyGuesses[i] = new int[] { 0 };
            }

            try
            {
                // Process the level with the dummy guesses.
                int spareGuesses = game.ProcessLevel(dummyGuesses);
                Console.WriteLine("Dummy Level Processed.");
                Console.WriteLine("Total spare guesses: " + spareGuesses);
                Console.WriteLine("Player's total score: " + game.Player.getScore());
                Console.WriteLine("Levels completed: " + game.Player.getLevelsCompleted());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing level: " + ex.Message);
            }

            // Save game state for persistence.
            GameStateManager stateManager = new GameStateManager();
            stateManager.saveState(game);

            Console.WriteLine("Game state saved. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
