using System;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    public partial class MainForm : Form
    {
        private NumberCruncherGame game; 
        private Difficulty selectedDifficulty;
        private string randomNumberToDisplay;

        // Constructor accepting a NumberCruncherGame instance
        public MainForm(NumberCruncherGame game, Difficulty difficulty)
        {
            InitializeComponent();
            this.game = game; // Store game instance
            this.selectedDifficulty = difficulty;
            randomNumberToDisplay = GenerateSpecialRandomNumber().ToString();
            // Very Hacky implementation, Simply to adhere to section D of the specification, this is temporary and
            // will be removed in later submissions of the assignment
            TextBox[] textBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            foreach (var textBox in textBoxes)
            {
                textBox.Text =  GenerateSpecialRandomNumber().ToString();
            }

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
            // fill the list with numbers within the range specified by the difficulty
            switch (selectedDifficulty)
            {
                case Difficulty.EASY:
                    // range of 1-10 and find the mode
                    maxRange = 10;
                    break;
                case Difficulty.MODERATE:
                    // range of 1-100 and find the mode
                    maxRange = 100;
                    break;
                case Difficulty.DIFFICULT:
                    // range of 1-1000 and find the mode
                    maxRange = 1000;
                    break;


            }
            // fill the list with random numbers from a range of 1-maxRange
            for (int index = 0; index < 1000; index++)
            {
                randomNumbers.Add(random.Next(minRange, maxRange));
            }
            // find the mode in the list of random numbers (LINQ Solution)
            int mode = randomNumbers.GroupBy(value => value)
                .OrderByDescending(group => group.Count())
                .First()
                .Key;
            return mode;
        }
    }
}
