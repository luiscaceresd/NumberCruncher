using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using NumberCruncherClient;

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

            // Initialize the specialNumbers list to store random numbers for each track

            

            // Clear any existing guesses in the textboxes (just for a fresh start)
            TextBox[] textBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            foreach (var textBox in textBoxes)
            {
                textBox.Clear();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateTrackVisibility(selectedDifficulty);
        }

        private void UpdateTrackVisibility(Difficulty difficulty)
        {
            // Determine the number of tracks to display based on difficulty
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
            // Array of guess input fields and corresponding track indicators
            TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            PictureBox[] trackIndicators = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };

            bool allCorrect = true;
            string debugMessage = "Track Results:\n";
            Track[] tracks = game.GetTracks();

            // Iterate over each Track instance directly
            for (int i = 0; i < tracks.Length; i++)
            {
                Track track = tracks[i]; // Directly access Track instance

                if (guessTextBoxes[i].Visible) // Check only visible tracks
                {
                    bool isCorrect = int.TryParse(guessTextBoxes[i].Text, out int userGuess) && track.CheckGuess(userGuess);

                    // If the guess is correct, update the indicator with a green check
                    if (isCorrect)
                    {
                        trackIndicators[i].Image = Properties.Resources.Green_check; // Correct guess
                        debugMessage += $"Track {i + 1}: Correct!\n";
                    }
                    else
                    {
                        // If incorrect, update the indicator with a red X
                        trackIndicators[i].Image = Properties.Resources.Red_X; // Incorrect guess
                        debugMessage += $"Track {i + 1}: Incorrect (Guessed: {userGuess})\n";
                        allCorrect = false;
                    }
                }
            }

            // Show debugging info
            MessageBox.Show(debugMessage, "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Check for win condition
            if (allCorrect)
            {
                MessageBox.Show("All tracks are correct! You win!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Some tracks are incorrect. Try again.", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Handle the Save button click here
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Closes the Main Screen Form and returns to the previous "screen" 
            PlayerSetupForm playerSetupForm = new PlayerSetupForm();
            playerSetupForm.Show();
            this.Close();
        }

        // Generate a random special number based on difficulty
        private int GenerateSpecialRandomNumber()
        {
            Random random = new Random();
            int minRange = 1;
            int maxRange = 1;
            List<int> randomNumbers = new List<int>();

            // Set range based on difficulty
            switch (selectedDifficulty)
            {
                case Difficulty.EASY:
                    maxRange = 10;
                    break;
                case Difficulty.MODERATE:
                    maxRange = 100;
                    break;
                case Difficulty.DIFFICULT:
                    maxRange = 1000;
                    break;
            }

            // Fill the list with random numbers
            for (int index = 0; index < 1000; index++)
            {
                randomNumbers.Add(random.Next(minRange, maxRange));
            }

            // Find and return the mode in the list of random numbers (most frequent number)
            int mode = randomNumbers.GroupBy(value => value)
                .OrderByDescending(group => group.Count())
                .First()
                .Key;
            return mode;
        }
    }
}
