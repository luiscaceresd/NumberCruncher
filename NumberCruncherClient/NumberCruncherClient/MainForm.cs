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
    /// The primary user interface for playing the NumberCruncher game.
    /// Handles user inputs, guesses, validation and level progression.
    /// </summary>
    public partial class MainForm : Form
    {
        private NumberCruncherGame currentGame;     // The current game instance.
        private Difficulty selectedDifficulty;      // Currently selected difficulty.
        private int[] remainingGuesses;             // The remaining guess counts per track.

        // Gets the current maximum number in the guessing range.
        private int CurrentMaxRange => currentGame.GetCurrentMaxRange();

        /// <summary>
        /// Constructor to initialize the main form with a game and chosen difficulty.
        /// </summary>
        /// <param name="game">The current NumberCruncherGame instance.</param>
        /// <param name="difficulty">The selected difficulty level.</param>
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
                MessageBox.Show("Error restoring game state: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Clear guess inputs on startup.
            TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
            foreach (var textBox in guessTextBoxes)
            {
                textBox.Clear();
            }
        }

        /// <summary>
        /// On form load, set up UI elements and initialize remaining guess counts.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateTrackVisibility(selectedDifficulty);

                // Default allowed attempts based on difficulty.
                int defaultAttempts = selectedDifficulty switch
                {
                    Difficulty.EASY => 5,
                    Difficulty.MODERATE => 7,
                    Difficulty.DIFFICULT => 11,
                    _ => 0
                };

                Track[] tracks = currentGame.GetTracks();
                // Determine if this is a resumed game (if guess history exists).
                bool isResumedGame = tracks != null && tracks.Length > 0 &&
                                     tracks[0].guessHistory != null && tracks[0].guessHistory.Count > 0;

                for (int i = 0; i < tracks.Length; i++)
                {
                    if (isResumedGame)
                    {
                        // Use saved allowed attempts from the track.
                        remainingGuesses[i] = tracks[i].GetAllowedAttempts();
                    }
                    else
                    {
                        // Set new game values.
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
                MessageBox.Show("Error during form load: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates which track boxes are visible and sets the displayed guess counts.
        /// </summary>
        /// <param name="difficulty">The current difficulty level.</param>
        private void UpdateTrackVisibility(Difficulty difficulty)
        {
            // Number of tracks based on difficulty.
            int trackCount = difficulty switch
            {
                Difficulty.EASY => 3,
                Difficulty.MODERATE => 5,
                Difficulty.DIFFICULT => 7,
                _ => 0,
            };

            // Get the group boxes containing track UI elements.
            GroupBox[] trackGroupBoxes = { track1, track2, track3, track4, track5, track6, track7 };
            Label[] guessCountLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

            // Hide all tracks initially.
            foreach (var groupBox in trackGroupBoxes)
            {
                groupBox.Visible = false;
            }

            // Determine whether this is a resumed game.
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

            // Set visibility and update labels.
            for (int i = 0; i < trackCount; i++)
            {
                trackGroupBoxes[i].Visible = true;
                if (isResumed)
                    guessCountLabels[i].Text = $"Guesses: {tracks[i].GetAllowedAttempts()}";
                else
                    guessCountLabels[i].Text = $"Guesses: {defaultAttempts}";
            }
        }

        /// <summary>
        /// Dictionary to track which tracks have been completed.
        /// Key: track index, Value: the correct guess.
        /// </summary>
        private Dictionary<int, int> completedTracks = new Dictionary<int, int>();

        /// <summary>
        /// Handles the guess submission, including validating inputs, processing each track, and updating UI.
        /// Validates that inputs are numeric and within the allowed range.
        /// </summary>
        private void btnGuess_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve arrays of UI controls.
                TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
                PictureBox[] trackIndicatorPics = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
                ListBox[] historyListBoxes = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
                Label[] feedbackLabels = { lblFeedback1, lblFeedback2, lblFeedback3, lblFeedback4, lblFeedback5, lblFeedback6, lblFeedback7 };
                Label[] guessCountLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

                // Pre-validate each visible track input.
                for (int i = 0; i < guessTextBoxes.Length; i++)
                {
                    if (guessTextBoxes[i].Visible)
                    {
                        string input = guessTextBoxes[i].Text.Trim();
                        if (string.IsNullOrEmpty(input))
                        {
                            MessageBox.Show($"Please enter a number for Track {i + 1}.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (!int.TryParse(input, out int numericGuess))
                        {
                            MessageBox.Show($"Track {i + 1} input is not numeric. Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (numericGuess < 1 || numericGuess > CurrentMaxRange)
                        {
                            MessageBox.Show($"Track {i + 1} guess must be between 1 and {CurrentMaxRange}.", "Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                bool allTracksCorrect = true;
                StringBuilder debugInfo = new StringBuilder("Track Results:\n");
                Track[] tracks = currentGame.GetTracks();

                // Load images for indicators.
                Image imgGreenCheck = Properties.Resources.GreenCheck;
                Image imgRedX = Properties.Resources.RedX;
                Image imgBlank = Properties.Resources.Blank;

                // Clear previous debug messages.
                lblResult.Text = string.Empty;

                // Process each track's guess.
                for (int i = 0; i < tracks.Length; i++)
                {
                    Track currentTrack = tracks[i];
                    if (guessTextBoxes[i].Visible)
                    {
                        int guessValue = int.Parse(guessTextBoxes[i].Text.Trim());
                        bool guessIsCorrect = currentTrack.CheckGuess(guessValue);

                        // Update the guess history.
                        currentTrack.guessHistory.Add(guessValue);
                        historyListBoxes[i].Items.Add(guessValue);

                        // Get and show feedback.
                        string responseFeedback = currentTrack.GetFeedback(guessValue);
                        feedbackLabels[i].Text = responseFeedback;

                        if (guessIsCorrect)
                        {
                            // If track not already completed, deduct one guess.
                            if (!completedTracks.ContainsKey(i))
                            {
                                remainingGuesses[i]--;
                                currentTrack.SetAllowedAttempts(remainingGuesses[i]);
                                guessCountLabels[i].Text = $"Guesses: {remainingGuesses[i]}";

                                trackIndicatorPics[i].Image = imgGreenCheck;
                                guessTextBoxes[i].Enabled = false;
                                completedTracks[i] = guessValue;
                            }
                        }
                        else
                        {
                            remainingGuesses[i]--;
                            currentTrack.SetAllowedAttempts(remainingGuesses[i]);
                            guessCountLabels[i].Text = $"Guesses: {remainingGuesses[i]}";

                            // Check if no guesses left.
                            if (remainingGuesses[i] <= 0)
                            {
                                trackIndicatorPics[i].Image = imgRedX;
                                guessTextBoxes[i].Enabled = false;
                                feedbackLabels[i].Text = " | Out of Lives";
                                debugInfo.AppendLine($"Track {i + 1}: Out of Lives - Game Over");

                                DialogResult restartChoice = MessageBox.Show("Game Over! You've run out of lives on a track. Start over? (Your Save File has been deleted)",
                                    "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                // Delete save file if exists.
                                string savePath = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                    "NumberCruncherGame",
                                    $"gamestate_{currentGame.Player.getInitials()}.json"
                                );
                                if (File.Exists(savePath))
                                {
                                    File.Delete(savePath);
                                }

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
                            trackIndicatorPics[i].Image = imgRedX;
                            debugInfo.AppendLine($"Track {i + 1}: Incorrect (Guessed: {guessValue})");
                            allTracksCorrect = false;
                        }
                    }
                    else
                    {
                        trackIndicatorPics[i].Image = imgBlank;
                        feedbackLabels[i].Text = string.Empty;
                    }
                }

                lblResult.Text = debugInfo.ToString();

                // Check if all visible tracks are finalized.
                bool allFinalized = true;
                for (int i = 0; i < tracks.Length; i++)
                {
                    if (guessTextBoxes[i].Enabled)
                    {
                        allFinalized = false;
                        break;
                    }
                }

                if (allFinalized)
                {
                    // Process level completion.
                    int[][] allGuesses = historyListBoxes.Select(lb => lb.Items.Cast<int>().ToArray()).ToArray();
                    int spareGuesses = currentGame.ProcessLevel(allGuesses);
                    currentGame.nextLevel();
                    currentGame.SaveGame();

                    MessageBox.Show($"Level Complete! Score: {currentGame.Player.getScore()} points. Game saved!",
                        "Level Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lblScore.Text = $"Score: {currentGame.Player.getScore()}";
                    lblRange.Text = $"Range: 1 - {CurrentMaxRange}";
                    lblDifficulty.Text = $"Difficulty: {currentGame.Difficulty}";

                    ReloadForNextLevel();
                }

                lblResult.Text += allTracksCorrect ? "\nAll tracks correct! You win!" : "\nSome tracks are incorrect. Try again.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing guess: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Prepares the UI for the next level by resetting track values and UI controls.
        /// </summary>
        private void ReloadForNextLevel()
        {
            try
            {
                UpdateTrackVisibility(selectedDifficulty);

                // Base lives per track based on difficulty.
                int baseLives = selectedDifficulty switch
                {
                    Difficulty.EASY => 5,
                    Difficulty.MODERATE => 7,
                    Difficulty.DIFFICULT => 11,
                    _ => 0
                };

                Track[] tracks = currentGame.GetTracks();
                TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
                PictureBox[] trackIndicators = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
                ListBox[] historyListBoxes = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
                Label[] guessLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };

                for (int i = 0; i < tracks.Length; i++)
                {
                    if (guessTextBoxes[i].Visible)
                    {
                        // Reset remaining guesses by adding the base number.
                        remainingGuesses[i] += baseLives;
                        tracks[i].SetAllowedAttempts(remainingGuesses[i]);

                        guessTextBoxes[i].Enabled = true;
                        guessTextBoxes[i].Clear();
                        trackIndicators[i].Image = Properties.Resources.Blank;
                        historyListBoxes[i].Items.Clear();
                        guessLabels[i].Text = $"Guesses: {remainingGuesses[i]}";
                    }
                }

                lblResult.Text = string.Empty;
                completedTracks.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting up next level: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exits the game; displays the setup form before closing the current form.
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
                MessageBox.Show("Error closing game: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Generates a special random number by determining the mode from 1000 random values.
        /// </summary>
        /// <returns>A unique mode number within the current range.</returns>
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

                // Generate 1000 random numbers.
                for (int i = 0; i < 1000; i++)
                {
                    randomValues.Add(rnd.Next(minRange, maxRange));
                }

                // Calculate the mode (most frequent number).
                int computedMode = randomValues.GroupBy(val => val)
                    .OrderByDescending(g => g.Count())
                    .First()
                    .Key;
                return computedMode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating random number: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>
        /// Restores the game state in the UI from the saved data in the current game.
        /// </summary>
        private void RestoreFromGameState()
        {
            try
            {
                Track[] tracks = currentGame.GetTracks();
                Label[] guessLabels = { lblGuesses1, lblGuesses2, lblGuesses3, lblGuesses4, lblGuesses5, lblGuesses6, lblGuesses7 };
                PictureBox[] trackIndicators = { picTrack1, picTrack2, picTrack3, picTrack4, picTrack5, picTrack6, picTrack7 };
                TextBox[] guessTextBoxes = { txtGuess1, txtGuess2, txtGuess3, txtGuess4, txtGuess5, txtGuess6, txtGuess7 };
                ListBox[] historyListBoxes = { lstHistory1, lstHistory2, lstHistory3, lstHistory4, lstHistory5, lstHistory6, lstHistory7 };
                Label[] feedbackLabels = { lblFeedback1, lblFeedback2, lblFeedback3, lblFeedback4, lblFeedback5, lblFeedback6, lblFeedback7 };

                int defaultAttempts = selectedDifficulty switch
                {
                    Difficulty.EASY => 5,
                    Difficulty.MODERATE => 7,
                    Difficulty.DIFFICULT => 11,
                    _ => 0
                };

                // For each track, restore remaining guesses and history.
                for (int i = 0; i < tracks.Length; i++)
                {
                    remainingGuesses[i] = tracks[i].GetAllowedAttempts();

                    guessTextBoxes[i].Enabled = true;
                    guessTextBoxes[i].Clear();
                    trackIndicators[i].Image = Properties.Resources.Blank;
                    feedbackLabels[i].Text = string.Empty;
                    historyListBoxes[i].Items.Clear();

                    // Display previously recorded guesses.
                    foreach (var guess in tracks[i].guessHistory)
                    {
                        historyListBoxes[i].Items.Add(guess);
                    }
                    guessLabels[i].Text = $"Guesses: {remainingGuesses[i]}";
                }

                lblScore.Text = $"Score: {currentGame.Player.getScore()}";
                lblRange.Text = $"Range: 1 - {currentGame.GetCurrentMaxRange()}";
                lblDifficulty.Text = $"Difficulty: {currentGame.Difficulty}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restoring game state: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves the current game state.
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
                MessageBox.Show("Failed to save game: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
