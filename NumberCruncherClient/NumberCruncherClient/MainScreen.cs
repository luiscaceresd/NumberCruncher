using System;
using System.Drawing.Imaging;
using System.Text;
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
            // Array of guess input fields, track indicators, history list boxes, and feedback labels
            TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            PictureBox[] trackIndicators = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
            ListBox[] lstHistories = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
            Label[] feedbackLabels = { lblFeedback1, lblFeedback2, lblFeedback3, lblFeedback4, lblFeedback5, lblFeedback6, lblFeedback7 };

            bool allCorrect = true;
            StringBuilder debugMessage = new StringBuilder("Track Results:\n");
            Track[] tracks = game.GetTracks();

            // Load images
            Image greenCheck = Properties.Resources.GreenCheck;
            Image redX = Properties.Resources.RedX;
            Image blank = Properties.Resources.Blank;

            // Clear previous debug messages
            lblResult.Text = "";

            // Iterate over each Track instance directly
            for (int i = 0; i < tracks.Length; i++)
            {
                Track track = tracks[i]; // Directly access Track instance

                if (guessTextBoxes[i].Visible) // Check only visible tracks
                {
                    if (int.TryParse(guessTextBoxes[i].Text, out int userGuess))
                    {
                        bool isCorrect = track.CheckGuess(userGuess);
                        string feedback = track.GetFeedback(userGuess); // Get feedback

                        // Store the guess in the track's history ListBox
                        lstHistories[i].Items.Add(userGuess);

                        // Display feedback in the corresponding label
                        feedbackLabels[i].Text = feedback;

                        if (isCorrect)
                        {
                            trackIndicators[i].Image = greenCheck;
                            debugMessage.AppendLine($"Track {i + 1}: Correct!");
                        }
                        else
                        {
                            trackIndicators[i].Image = redX;
                            debugMessage.AppendLine($"Track {i + 1}: Incorrect (Guessed: {userGuess})");
                            allCorrect = false;
                        }
                    }
                    else
                    {
                        feedbackLabels[i].Text = "Invalid"; // Indicate invalid input
                        debugMessage.AppendLine($"Track {i + 1}: Invalid Input");
                    }
                }
                else
                {
                    trackIndicators[i].Image = blank;
                    feedbackLabels[i].Text = ""; // Clear feedback for inactive tracks
                }
            }

            // Display debugging info in lblResult
            lblResult.Text = debugMessage.ToString();

            // Check for win condition
            lblResult.Text += allCorrect ? "\nAll tracks are correct! You win!" : "\nSome tracks are incorrect. Try again.";
        }

        // Utility function to adjust image opacity
       

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
        // Generate an array of 1000 numbers within the range. and find the mode.
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
