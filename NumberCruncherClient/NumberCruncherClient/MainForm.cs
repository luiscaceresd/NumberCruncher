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
        private int[] trackLives = new int[7];
        private int currentMaxRange => game.GetCurrentMaxRange();

        // Constructor accepting a NumberCruncherGame instance
        public MainForm(NumberCruncherGame game, Difficulty difficulty)
        {
            InitializeComponent();
           
            this.game = game; // Store game instance
            this.selectedDifficulty = difficulty;
            RestoreFromGameState();

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
            // Allocation of Initial lives
            int startingLives = selectedDifficulty switch
            {
                Difficulty.EASY => 5,
                Difficulty.MODERATE => 7,
                Difficulty.DIFFICULT => 11,
                _ => 0
            };
            // assigning starting lives to every active track
            for (int index = 0; index < game.GetTracks().Length; index++)
            {
                trackLives[index] = startingLives;
            }
            lblDifficulty.Text = $"Difficulty: {selectedDifficulty}";
            lblScore.Text = $"Score : {game.Player.getScore()}";
            lblRange.Text = $"Range: 1 - {currentMaxRange}";
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


        private Dictionary<int, int> correctGuesses = new Dictionary<int, int>();

        private void btnGuess_Click(object sender, EventArgs e)
        {
            // Array of guess input fields, track indicators, history list boxes, and feedback labels
            TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            PictureBox[] trackIndicators = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
            ListBox[] lstHistories = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
            Label[] feedbackLabels = { lblFeedback1, lblFeedback2, lblFeedback3, lblFeedback4, lblFeedback5, lblFeedback6, lblFeedback7 };
            Label[] guessLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

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
                        // Store the guess in the track's history
                        track.guessHistory.Add(userGuess); 

                        string feedback = track.GetFeedback(userGuess); // Get feedback



                        // Store the guess in the track's history ListBox
                        lstHistories[i].Items.Add(userGuess);

                        // Display feedback in the corresponding label
                        feedbackLabels[i].Text = feedback;

                        if (isCorrect)
                        {
                            // Only deduct a guess if the track wasn't already completed
                            if (!correctGuesses.ContainsKey(i))
                            {
                                trackLives[i]--; // Deduct a guess
                                guessLabels[i].Text = $"Guesses: {trackLives[i]}"; // Update label to reflect new guess count

                                trackIndicators[i].Image = greenCheck;
                                guessTextBoxes[i].Enabled = false;

                                // Store correct guess for this track
                                correctGuesses[i] = userGuess;
                            }
                        }

                        else
                        {
                            trackLives[i]--;
                            guessLabels[i].Text = $"Guesses: {trackLives[i]}";

                            if (trackLives[i] <= 0 && !isCorrect)
                            {
                                trackIndicators[i].Image = redX;
                                guessTextBoxes[i].Enabled = false;
                                feedbackLabels[i].Text = " | Out of Lives";
                                debugMessage.AppendLine($"Track {i + 1}: Out of Lives - Game Over");

                                DialogResult result = MessageBox.Show("Game Over! You ran out of lives on the track. Would you like to start over?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    // Show PlayerSetupForm
                                    PlayerSetupForm playerSetupForm = new PlayerSetupForm();
                                    playerSetupForm.Show();

                                    // Close the current MainForm
                                    this.Close();
                                }
                                else
                                {
                                    // If the player selects No, just close the application
                                    Application.Exit();
                                }
                            }

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

            bool allTracksFinished = true;
            // validate that we truly have won on all tracks.
            for (int index = 0; index < tracks.Length; index++)
            {
                if (guessTextBoxes[index].Enabled)
                {
                    allTracksFinished = false;
                    break;
                }
            }

            if (allTracksFinished)
            {
                int[][] guessesPerTrack = lstHistories
                    .Select(lst => lst.Items.Cast<int>().ToArray())
                    .ToArray();

                int spare = game.ProcessLevel(guessesPerTrack);
                game.nextLevel();
                game.SaveGame();
                MessageBox.Show($"Level Complete! You now have {game.Player.getScore()} points. Your Game has Been Saved!",
                     "Level Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);


                lblScore.Text = $"Score : {game.Player.getScore()}";
                lblRange.Text = $"Range: 1 - {currentMaxRange}";
                lblDifficulty.Text = $"Difficulty: {game.Difficulty}";

                ReloadForNextLevel();
            }

            // Check for win condition
            lblResult.Text += allCorrect ? "\nAll tracks are correct! You win!" : "\nSome tracks are incorrect. Try again.";
        }

        // Utility function to adjust image opacity

        private void ReloadForNextLevel()
        {
            UpdateTrackVisibility(selectedDifficulty);

            int baseLives = selectedDifficulty switch
            {
                Difficulty.EASY => 5,
                Difficulty.MODERATE => 7,
                Difficulty.DIFFICULT => 11,
                _ => 0
            };

            Track[] newTracks = game.GetTracks();

            TextBox[] textBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            PictureBox[] indicators = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
            ListBox[] histories = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
            Label[] guessLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

            for (int index = 0; index < newTracks.Length; index++)
            {
                if (textBoxes[index].Visible) // Only reset visible tracks
                {
                    // Carry over remaining lives, but ensure it's not reset to the base value
                    trackLives[index] += baseLives;

                    // Reset fields for the new level
                    textBoxes[index].Enabled = true;
                    textBoxes[index].Clear();
                    indicators[index].Image = Properties.Resources.Blank;
                    histories[index].Items.Clear();
                    guessLabels[index].Text = $"Guesses: {trackLives[index]}";  // Display updated guesses count
                }
            }

            lblResult.Text = "";
            correctGuesses.Clear(); // Reset the dictionary of correct guesses
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
        private void RestoreFromGameState()
        {
            Track[] tracks = game.GetTracks();
            Label[] guessLabels = {lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };
            PictureBox[] trackIndicators = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
            TextBox[] textBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            ListBox[] histories = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
            Label[] feedbacks = { lblFeedback1, lblFeedback2, lblFeedback3, lblFeedback4, lblFeedback5, lblFeedback6, lblFeedback7 };
            int lives = selectedDifficulty switch
            {
                Difficulty.EASY => 5,
                Difficulty.MODERATE => 7,
                Difficulty.DIFFICULT => 11,
                _ => 0
            };

            for (int index = 0; index < tracks.Length; index++)
            {
                trackLives[index] = lives;

                textBoxes[index].Enabled = true;
                textBoxes[index].Clear();
                trackIndicators[index].Image = Properties.Resources.Blank;
                feedbacks[index].Text = string.Empty;
                histories[index].Items.Clear();
                guessLabels[index].Text = $"Guesses : {lives}";

                guessLabels[index].Text = $"Guesses : {trackLives[index]}";

                
                if (tracks[index].guessHistory.Contains(tracks[index].GetMode()))
                {
                    trackIndicators[index].Image = Properties.Resources.GreenCheck;
                    guessLabels[index].Text = $"Guesses : {trackLives[index]}";
                    trackIndicators[index].Enabled = false;
                }
            }
           

            lblScore.Text = $"Score : {game.Player.getScore()}";
            lblRange.Text = $"Range: 1 - {game.GetCurrentMaxRange()}";
            lblDifficulty.Text = $"Difficulty: {game.Difficulty}";
        }
    }


}
