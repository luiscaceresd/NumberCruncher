namespace NumberCruncherClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Show the splash screen first
            using (SplashScreen splash = new SplashScreen())
            {
                splash.ShowDialog();
            }

            // After splash screen, show PlayerSetup instead of MainForm
            Application.Run(new PlayerSetupForm());
        }
    }
}
