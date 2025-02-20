using System;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            // Optionally set the timer interval in code (e.g., 3000 milliseconds = 3 seconds)
            timer1.Interval = 2000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer and close the splash screen.
            timer1.Stop();
            this.Close();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            // Start the timer when the splash screen loads.
            timer1.Start();
        }
    }
}
