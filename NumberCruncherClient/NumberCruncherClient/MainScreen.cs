using System;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    public partial class MainForm : Form
    {
        private NumberCruncherGame game; // Remove nullable (?) and ensure it's initialized
        private int trackCount;

        // Constructor accepting a NumberCruncherGame instance
        public MainForm(NumberCruncherGame game)
        {
            InitializeComponent();
            this.game = game; // Store game instance
            this.trackCount = game.TrackCount; // Assuming TrackCount is part of NumberCruncherGame
            SetupTracksBasedOnDifficulty(trackCount); // Setup the tracks based on difficulty
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Event handler for the Start Game button.
        /// Initializes the game by setting player initials and difficulty,
        /// then dynamically creates input controls for each track.
        /// </summary>
        private void SetupTracksBasedOnDifficulty(int trackCount)
        {
            // Hide all tracks initially
            foreach (Control control in panelTracks.Controls)
            {
                if (control is GroupBox track)
                {
                    track.Visible = false;
                }
            }

            // Show the appropriate number of tracks based on trackCount
            for (int i = 0; i < trackCount; i++)
            {
                if (i < panelTracks.Controls.Count)
                {
                    GroupBox trackGroupBox = panelTracks.Controls[i] as GroupBox;
                    if (trackGroupBox != null)
                    {
                        trackGroupBox.Visible = true;  // Make the track visible
                    }
                }
            }
        }


        // Call this function when difficulty is changed
        public void SetTrackCountBasedOnDifficulty(Difficulty difficulty)
        {
            // Set track count based on selected difficulty
            switch (difficulty)
            {
                case Difficulty.EASY:
                    trackCount = 3;  // Easy: 3 tracks
                    break;
                case Difficulty.MODERATE:
                    trackCount = 5;  // Moderate: 5 tracks
                    break;
                case Difficulty.DIFFICULT:
                    trackCount = 7;  // Difficult: 7 tracks
                    break;
                default:
                    trackCount = 3; // Default to Easy if no difficulty is selected
                    break;
            }

            // Setup tracks again based on the new difficulty setting
            SetupTracksBasedOnDifficulty(trackCount);
        }

        public void UpdateTrackVisibility(Difficulty difficulty)
        {
            // Get the number of tracks based on difficulty
            int trackCount = difficulty switch
            {
                Difficulty.EASY => 3,
                Difficulty.MODERATE => 5,
                Difficulty.DIFFICULT => 7,
                _ => 0,
            };

            // Assuming you have a container panel called 'panelTracks' where your track GroupBoxes are added
            for (int i = 0; i < panelTracks.Controls.Count; i++)
            {
                // Hide all controls first
                var trackControl = panelTracks.Controls[i] as GroupBox;
                if (trackControl != null)
                {
                    trackControl.Visible = false;
                }
            }

            // Show the correct number of tracks based on the difficulty level
            for (int i = 0; i < trackCount; i++)
            {
                var trackControl = panelTracks.Controls[i] as GroupBox;
                if (trackControl != null)
                {
                    trackControl.Visible = true;
                }
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
