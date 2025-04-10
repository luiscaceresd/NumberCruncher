using System;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    /// <summary>
    /// A simple splash screen that displays briefly upon starting the game.
    /// </summary>
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            // Set timer interval (e.g., 2000ms = 2 seconds).
            timer1.Interval = 2000;
        }

        /// <summary>
        /// When the timer ticks, stop it and close the splash screen.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }

        /// <summary>
        /// Starts the timer when the splash screen loads.
        /// </summary>
        private void SplashScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
