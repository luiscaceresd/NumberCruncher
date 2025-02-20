using System;
using System.Linq;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    public partial class MainForm : Form
    {
        // Field to hold the game instance from the server backend.
        private NumberCruncherGame? game;

        private int trackCount;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Event handler for the Start Game button.
        /// Initializes the game by setting player initials and difficulty,
        /// then dynamically creates input controls for each track.
        /// </summary>

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            // Create a new game instance.
            game = new NumberCruncherGame();

            // Set the player's initials from the txtInitials TextBox.
            game.Player.setInitials(txtInitials.Text.Trim());

            // Determine difficulty from the ComboBox selection.
            string difficultyStr = cmbDifficulty.SelectedItem?.ToString() ?? "EASY";
            Difficulty selectedDifficulty;
            switch (difficultyStr.ToUpper())
            {
                case "EASY":
                    selectedDifficulty = Difficulty.EASY;
                    trackCount = 3;
                    break;
                case "MODERATE":
                    selectedDifficulty = Difficulty.MODERATE;
                    trackCount = 5;
                    break;
                case "DIFFICULT":
                    selectedDifficulty = Difficulty.DIFFICULT;
                    trackCount = 7;
                    break;
                default:
                    selectedDifficulty = Difficulty.EASY;
                    trackCount = 3;
                    break;
            }

            // Set the selected difficulty in the game.
            game.Difficulty = selectedDifficulty;

            // Initialize the game (this sets up the tracks for the first level).
            game.startGame();

            // Create input controls for level guesses based on trackCount.
            SetupLevelInputControls(trackCount);

            // Clear any previous result.
            lblResult.Text = "";
        }
        private void SetupLevelInputControls(int trackCount)
        {
            // Clear any existing controls.
            panelTracks.Controls.Clear();

            // Create controls for each track.
            for (int i = 0; i < trackCount; i++)
            {
                // Create a Label for the track.
                Label lbl = new Label();
                lbl.Text = $"Track {i + 1} guesses (comma separated):";
                lbl.Top = i * 40;
                lbl.Left = 10;
                lbl.Width = 250;
                lbl.AutoSize = true;

                // Create a TextBox for the track.
                TextBox txt = new TextBox();
                txt.Name = $"txtTrack{i}";
                txt.Top = i * 40;
                txt.Left = 270;
                txt.Width = 200;

                // Add the controls to the panel.
                panelTracks.Controls.Add(lbl);
                panelTracks.Controls.Add(txt);
            }
        }

        private void btnSubmitLevel_Click(object sender, EventArgs e)
        {
            if (game == null)
            {
                MessageBox.Show("Please start the game first.");
                return;
            }

            // Process the guesses...
            int[][] guesses = new int[trackCount][];
            // ... (rest of your existing code)

            try
            {
                int spareGuesses = game.ProcessLevel(guesses);
                lblResult.Text = $"Level processed.\nSpare guesses: {spareGuesses}\n" +
                                 $"Total Score: {game.Player.getScore()}\n" +
                                 $"Levels Completed: {game.Player.getLevelsCompleted()}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing level: " + ex.Message);
            }
        }

        private void btnNextLevel_Click(object sender, EventArgs e)
        {
            if (game == null)
            {
                MessageBox.Show("Please start the game first.");
                return;
            }

            game.nextLevel();

            // Clear all TextBoxes in the panel for new input.
            foreach (Control c in panelTracks.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }
            MessageBox.Show("Next level started. Please enter new guesses for each track.");
        }

    }
}
