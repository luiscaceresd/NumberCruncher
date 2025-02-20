namespace NumberCruncherClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Show the splash screen before the main form.
            using (SplashScreen splash = new SplashScreen())
            {
                splash.ShowDialog();
            }

            // After the splash screen closes, run the main form.
            Application.Run(new MainForm());
        }
    }
}
