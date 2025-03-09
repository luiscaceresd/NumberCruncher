using System;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    public partial class MainForm : Form
    {
        private NumberCruncherGame game; 
        private Difficulty selectedDifficulty;

        // Constructor accepting a NumberCruncherGame instance
        public MainForm(NumberCruncherGame game, Difficulty difficulty)
        {
            InitializeComponent();
            this.game = game; // Store game instance
            this.selectedDifficulty = difficulty;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateTrackVisibility(selectedDifficulty);
        }



        private void UpdateTrackVisibility(Difficulty difficulty)
        {
            // Determine the number of tracks to display
            int trackCount = difficulty switch
            {
                Difficulty.EASY => 3,
                Difficulty.MODERATE => 5,
                Difficulty.DIFFICULT => 7,
                _ => 0,
            };

            int allowedAttempts = difficulty switch
            {
                Difficulty.EASY => 5,
                Difficulty.MODERATE => 7,
                Difficulty.DIFFICULT => 11,
                _ => 0,
            };

            // Explicitly reference each GroupBox track by name
            GroupBox[] trackBoxes =
            {
                track1, track2, track3, track4, track5, track6, track7
            };

            Label[] guessLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

            // Hide all tracks first
            foreach (var track in trackBoxes)
            {
                track.Visible = false;
            }

            // Show only the required number of tracks
            for (int index = 0; index < trackCount; index++)
            {
                trackBoxes[index].Visible = true;
                guessLabels[index].Text = $"Guesses: {allowedAttempts}";
            }
        }



        private void btnGuess_Click(object sender, EventArgs e)
        {
            // Handle the Guess button click here
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Handle the Save button click here
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes the player view
        }
    }
}
