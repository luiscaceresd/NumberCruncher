using System.Windows.Forms;

namespace NumberCruncherClient
{
    /// <summary>
    /// The application entry point for the NumberCruncher game.
    /// Displays a splash screen and then starts the player setup process.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Display the splash screen.
            using (SplashScreen splash = new SplashScreen())
            {
                splash.ShowDialog();
            }

            // Start the game by opening the player setup form.
            Application.Run(new PlayerSetupForm());
        }
    }
}
