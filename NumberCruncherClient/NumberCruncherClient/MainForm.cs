using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace NumberCruncherClient
{
    /// <summary>
    /// The primary UI for playing NumberCruncher.
    /// Handles user input, guess validation, level progression, and UI updates.
    /// Autosave is removed; the user must press Save manually.
    /// </summary>
    public partial class MainForm : Form
    {
        // Current game instance.
        private NumberCruncherGame currentGame;
        // The selected game difficulty.
        private Difficulty selectedDifficulty;
        // Array tracking remaining guesses per track (up to 7 tracks).
        private int[] remainingGuesses;
        // Shortcut to the current maximum allowed guess.
        private int CurrentMaxRange => currentGame.GetCurrentMaxRange();

        /// <summary>
        /// Initializes MainForm with the specified game state and difficulty.
        /// Attempts to restore saved UI state.
        /// </summary>
        public MainForm(NumberCruncherGame game, Difficulty difficulty)
        {
            InitializeComponent();
            currentGame = game;
            selectedDifficulty = difficulty;
            remainingGuesses = new int[7];

            try
            {
                RestoreFromGameState();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restoring game state: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Clear all guess inputs.
            TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            foreach (var tb in guessTextBoxes)
            {
                tb.Clear();
            }
        }

        /// <summary>
        /// On form load, update UI: track visibility and initialize remaining guesses.
        /// Uses defaults for a new game or restores saved values.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateTrackVisibility(selectedDifficulty);
                int defaultAttempts = selectedDifficulty switch
                {
                    Difficulty.EASY => 5,
                    Difficulty.MODERATE => 7,
                    Difficulty.DIFFICULT => 11,
                    _ => 0,
                };

                Track[] tracks = currentGame.GetTracks();
                bool isResumedGame = tracks != null && tracks.Length > 0 &&
                                     tracks[0].guessHistory != null && tracks[0].guessHistory.Count > 0;

                for (int i = 0; i < tracks.Length; i++)
                {
                    if (isResumedGame)
                        remainingGuesses[i] = tracks[i].GetAllowedAttempts();
                    else
                    {
                        remainingGuesses[i] = defaultAttempts;
                        tracks[i].SetAllowedAttempts(defaultAttempts);
                    }
                }
                lblDifficulty.Text = $"Difficulty: {selectedDifficulty}";
                lblScore.Text = $"Score: {currentGame.Player.getScore()}";
                lblRange.Text = $"Range: 1 - {CurrentMaxRange}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during form load: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates track panel visibility and guess count labels based on difficulty.
        /// When the range increases and difficulty changes, more tracks will be shown.
        /// </summary>
        private void UpdateTrackVisibility(Difficulty difficulty)
        {
            int trackCount = difficulty switch
            {
                Difficulty.EASY => 3,
                Difficulty.MODERATE => 5,
                Difficulty.DIFFICULT => 7,
                _ => 0,
            };

            GroupBox[] trackPanels = { track1, track2, track3, track4, track5, track6, track7 };
            Label[] guessCountLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

            foreach (var panel in trackPanels)
            {
                panel.Visible = false;
            }

            Track[] tracks = currentGame.GetTracks();
            int defaultAttempts = difficulty switch
            {
                Difficulty.EASY => 5,
                Difficulty.MODERATE => 7,
                Difficulty.DIFFICULT => 11,
                _ => 0,
            };
            bool isResumed = tracks != null && tracks.Length > 0 &&
                             tracks[0].guessHistory != null && tracks[0].guessHistory.Count > 0;

            for (int i = 0; i < trackCount; i++)
            {
                trackPanels[i].Visible = true;
                if (isResumed)
                    guessCountLabels[i].Text = $"Guesses: {tracks[i].GetAllowedAttempts()}";
                else
                    guessCountLabels[i].Text = $"Guesses: {defaultAttempts}";
            }
        }

        /// <summary>
        /// Dictionary tracking completed tracks (key: track index; value: correct guess).
        /// </summary>
        private Dictionary<int, int> completedTracks = new Dictionary<int, int>();

        /// <summary>
        /// Handles the Guess button click.
        /// Validates input on each visible and enabled track,
        /// deducts one attempt for every guess (correct or incorrect),
        /// and finalizes a track when a guess is made.
        /// If all visible tracks are correct, the level is completed.
        /// </summary>
        private void btnGuess_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
                PictureBox[] indicatorPictures = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
                ListBox[] historyLists = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
                Label[] feedbackLabels = { lblFeedback1, lblFeedback2, lblFeedback3, lblFeedback4, lblFeedback5, lblFeedback6, lblFeedback7 };
                Label[] guessCountLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

                // Validate inputs for visible and enabled tracks.
                for (int i = 0; i < guessTextBoxes.Length; i++)
                {
                    if (!guessTextBoxes[i].Visible || !guessTextBoxes[i].Enabled)
                        continue;  // Skip controls for unused tracks.

                    string input = guessTextBoxes[i].Text.Trim();
                    if (string.IsNullOrEmpty(input))
                    {
                        MessageBox.Show($"Please enter a number for Track {i + 1}.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!int.TryParse(input, out int numericGuess))
                    {
                        MessageBox.Show($"Track {i + 1} input must be numeric.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (numericGuess < 1 || numericGuess > CurrentMaxRange)
                    {
                        MessageBox.Show($"Track {i + 1} guess must be between 1 and {CurrentMaxRange}.", "Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                bool allTracksCorrect = true;
                StringBuilder debugInfo = new StringBuilder("Track Results:\n");
                Track[] tracks = currentGame.GetTracks();

                // Load indicator images.
                Image imgGreenCheck = Properties.Resources.GreenCheck;
                Image imgRedX = Properties.Resources.RedX;
                Image imgBlank = Properties.Resources.Blank;

                lblResult.Text = string.Empty; // Clear debug text.

                // Process guesses for each visible and enabled track.
                for (int i = 0; i < tracks.Length; i++)
                {
                    if (!guessTextBoxes[i].Visible || !guessTextBoxes[i].Enabled)
                        continue;

                    Track currentTrack = tracks[i];
                    int guessValue = int.Parse(guessTextBoxes[i].Text.Trim());

                    // Deduct one attempt for every guess.
                    remainingGuesses[i]--;
                    currentTrack.SetAllowedAttempts(remainingGuesses[i]);
                    guessCountLabels[i].Text = $"Guesses: {remainingGuesses[i]}";

                    // Record guess in history.
                    currentTrack.guessHistory.Add(guessValue);
                    historyLists[i].Items.Add(guessValue);

                    // Get directional feedback.
                    string feedback = currentTrack.GetFeedback(guessValue);
                    feedbackLabels[i].Text = feedback;

                    // Check if the guess is correct.
                    if (currentTrack.CheckGuess(guessValue))
                    {
                        indicatorPictures[i].Image = imgGreenCheck;
                        guessTextBoxes[i].Enabled = false;  // Finalize the track.
                        completedTracks[i] = guessValue;
                    }
                    else
                    {
                        // If incorrect and out of attempts, finalize the track.
                        if (remainingGuesses[i] <= 0)
                        {
                            indicatorPictures[i].Image = imgRedX;
                            guessTextBoxes[i].Enabled = false;
                            feedbackLabels[i].Text += " | Out of Lives";
                            debugInfo.AppendLine($"Track {i + 1}: Out of Lives - Game Over");

                            DialogResult restartChoice = MessageBox.Show(
                                "Game Over! You've run out of lives on a track. Start over? (Your save file has been deleted)",
                                "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            // Delete saved game file if exists.
                            string savePath = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                "NumberCruncherGame",
                                $"gamestate_{currentGame.Player.getInitials()}.json"
                            );
                            if (File.Exists(savePath))
                                File.Delete(savePath);

                            if (restartChoice == DialogResult.Yes)
                            {
                                PlayerSetupForm setupForm = new PlayerSetupForm();
                                setupForm.Show();
                                this.Close();
                                return;
                            }
                            else
                            {
                                Application.Exit();
                                return;
                            }
                        }
                        indicatorPictures[i].Image = imgRedX;
                        debugInfo.AppendLine($"Track {i + 1}: Incorrect (Guessed: {guessValue})");
                        allTracksCorrect = false;
                    }
                }

                // Display debugging info.
                lblResult.Text = debugInfo.ToString();

                // Check if all visible tracks are finalized.
                bool allFinalized = true;
                for (int i = 0; i < tracks.Length; i++)
                {
                    if (guessTextBoxes[i].Visible && guessTextBoxes[i].Enabled)
                    {
                        allFinalized = false;
                        break;
                    }
                }

                // Process level completion if every visible track is finalized and all were correct.
                if (allFinalized && allTracksCorrect)
                {
                    int[][] allGuesses = historyLists.Select(lb => lb.Items.Cast<int>().ToArray()).ToArray();
                    int spareGuesses = currentGame.ProcessLevel(allGuesses);
                    currentGame.nextLevel();

                    // Notify the player; remind manual save.
                    MessageBox.Show(
                        $"Level Complete! Score: {currentGame.Player.getScore()} points. Remember, you can save your game by clicking the Save button.",
                        "Level Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lblScore.Text = $"Score: {currentGame.Player.getScore()}";
                    lblRange.Text = $"Range: 1 - {CurrentMaxRange}";
                    lblDifficulty.Text = $"Difficulty: {currentGame.Difficulty}";

                    ReloadForNextLevel();
                }
                else if (allFinalized && !allTracksCorrect)
                {
                    MessageBox.Show("Some tracks were guessed incorrectly. No score awarded for this level.",
                        "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                lblResult.Text += allTracksCorrect
                    ? "\nAll tracks correct! You win!"
                    : "\nSome tracks are incorrect. Try again.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing guess: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Helper method to clean up inactive (hidden) track controls.
        /// Clears text and disables controls that are not visible.
        /// </summary>
        private void CleanupInactiveTrackControls()
        {
            TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            foreach (var tb in guessTextBoxes)
            {
                if (!tb.Visible)
                {
                    tb.Clear();
                    tb.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Resets UI elements for the next level.
        /// Updates remaining guesses, clears inputs and history, and resets visual indicators.
        /// </summary>
        private void ReloadForNextLevel()
        {
            try
            {
                UpdateTrackVisibility(selectedDifficulty);
                int baseAttempts = selectedDifficulty switch
                {
                    Difficulty.EASY => 5,
                    Difficulty.MODERATE => 7,
                    Difficulty.DIFFICULT => 11,
                    _ => 0,
                };

                Track[] tracks = currentGame.GetTracks();
                TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
                PictureBox[] indicatorPictures = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
                ListBox[] historyLists = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
                Label[] guessCountLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

                for (int i = 0; i < tracks.Length; i++)
                {
                    if (guessTextBoxes[i].Visible)
                    {
                        // Add base attempts to remaining guesses.
                        remainingGuesses[i] += baseAttempts;
                        tracks[i].SetAllowedAttempts(remainingGuesses[i]);

                        guessTextBoxes[i].Enabled = true;
                        guessTextBoxes[i].Clear();
                        indicatorPictures[i].Image = Properties.Resources.Blank;
                        historyLists[i].Items.Clear();
                        guessCountLabels[i].Text = $"Guesses: {remainingGuesses[i]}";
                    }
                }
                lblResult.Text = string.Empty;
                completedTracks.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting up next level: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Closes the current game and returns to the PlayerSetupForm.
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                PlayerSetupForm setupForm = new PlayerSetupForm();
                setupForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error closing game: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Generates a special random number by computing the mode from 1000 random numbers in the current range.
        /// </summary>
        /// <returns>A unique mode within the current range.</returns>
        private int GenerateSpecialRandomNumber()
        {
            try
            {
                Random rnd = new Random();
                int minRange = 1;
                int maxRange = 1;
                List<int> randomValues = new List<int>();

                // Set max range based on difficulty.
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

                for (int i = 0; i < 1000; i++)
                {
                    randomValues.Add(rnd.Next(minRange, maxRange));
                }

                int computedMode = randomValues.GroupBy(val => val)
                    .OrderByDescending(g => g.Count())
                    .First()
                    .Key;
                return computedMode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating random number: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>
        /// Restores UI state from the current game instance by populating guess history,
        /// resetting textboxes, and updating labels for score, range, and difficulty.
        /// </summary>
        private void RestoreFromGameState()
        {
            try
            {
                Track[] tracks = currentGame.GetTracks();
                Label[] guessLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };
                PictureBox[] indicatorPictures = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
                TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
                ListBox[] historyLists = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
                Label[] feedbackLabels = { lblFeedback1, lblFeedback2, lblFeedback3, lblFeedback4, lblFeedback5, lblFeedback6, lblFeedback7 };

                int defaultAttempts = selectedDifficulty switch
                {
                    Difficulty.EASY => 5,
                    Difficulty.MODERATE => 7,
                    Difficulty.DIFFICULT => 11,
                    _ => 0,
                };

                for (int i = 0; i < tracks.Length; i++)
                {
                    remainingGuesses[i] = tracks[i].GetAllowedAttempts();
                    guessTextBoxes[i].Enabled = true;
                    guessTextBoxes[i].Clear();
                    indicatorPictures[i].Image = Properties.Resources.Blank;
                    feedbackLabels[i].Text = string.Empty;
                    historyLists[i].Items.Clear();

                    foreach (var guess in tracks[i].guessHistory)
                    {
                        historyLists[i].Items.Add(guess);
                    }
                    guessLabels[i].Text = $"Guesses: {remainingGuesses[i]}";
                }

                lblScore.Text = $"Score: {currentGame.Player.getScore()}";
                lblRange.Text = $"Range: 1 - {currentGame.GetCurrentMaxRange()}";
                lblDifficulty.Text = $"Difficulty: {currentGame.Difficulty}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restoring game state: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves the current game state when the Save button is pressed.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GameStateManager saveManager = new GameStateManager();
                saveManager.SaveState(currentGame);
                MessageBox.Show("Game saved successfully!", "Save Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save game: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
