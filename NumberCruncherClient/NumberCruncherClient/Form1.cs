using System;
using System.Linq;
using System.Windows.Forms;
using NumberCruncherServer;

namespace NumberCruncherClient
{
    public partial class MainForm : Form
    {
        // Field to hold the game instance from the server backend.
        private NumberCruncherGame game;

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
            // Create an array to hold guesses for each track.
            int[][] guesses = new int[trackCount][];

            for (int i = 0; i < trackCount; i++)
            {
                // Find the TextBox for track i.
                TextBox txt = panelTracks.Controls.Find($"txtTrack{i}", false).FirstOrDefault() as TextBox;
                if (txt != null)
                {
                    // Split the input on commas and parse each number.
                    string[] parts = txt.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int[] trackGuesses = parts.Select(p =>
                    {
                        if (int.TryParse(p.Trim(), out int val))
                            return val;
                        return 0; // Default if parsing fails.
                    }).ToArray();
                    guesses[i] = trackGuesses;
                }
                else
                {
                    guesses[i] = new int[] { 0 };
                }
            }

            try
            {
                // Process the level with the collected guesses.
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
            // Advance to the next level.
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
