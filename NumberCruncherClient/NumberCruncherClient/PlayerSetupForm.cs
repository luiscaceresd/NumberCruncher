using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    /// <summary>
    /// Form for player setup. Allows entering player initials and selecting difficulty.
    /// Also supports loading a saved game.
    /// </summary>
    public partial class PlayerSetupForm : Form
    {
        public PlayerSetupForm()
        {
            InitializeComponent();
            // Restrict input to letters only.
            txtInitials.KeyPress += new KeyPressEventHandler(txtInitials_KeyPress);
        }

        /// <summary>
        /// Handles additional initialization during form load.
        /// </summary>
        private void PlayerSetupForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Additional initialization code (if needed).
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Restricts the initials textbox to allow only letter input.
        /// </summary>
        private void txtInitials_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input processing error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Starts a game with EASY difficulty.
        /// </summary>
        private void btnEasy_Click(object sender, EventArgs e)
        {
            try
            {
                StartGame(Difficulty.EASY);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting EASY game: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Starts a game with MODERATE difficulty.
        /// </summary>
        private void btnModerate_Click(object sender, EventArgs e)
        {
            try
            {
                StartGame(Difficulty.MODERATE);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting MODERATE game: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Starts a game with DIFFICULT difficulty.
        /// </summary>
        private void btnDifficult_Click(object sender, EventArgs e)
        {
            try
            {
                StartGame(Difficulty.DIFFICULT);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting DIFFICULT game: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Validates input and starts a new game with the specified difficulty.
        /// </summary>
        /// <param name="selectedDifficulty">The chosen difficulty level.</param>
        private void StartGame(Difficulty selectedDifficulty)
        {
            try
            {
                string playerInitials = txtInitials.Text;
                if (string.IsNullOrWhiteSpace(playerInitials))
                {
                    MessageBox.Show("Enter your initials before starting the game.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ensure initials contain only letters.
                if (!playerInitials.All(char.IsLetter))
                {
                    MessageBox.Show("Initials must contain only letters.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a new game instance.
                NumberCruncherGame newGame = new NumberCruncherGame();
                newGame.Player.setInitials(playerInitials);
                newGame.Difficulty = selectedDifficulty;
                newGame.startGame();

                // Open the main game form.
                MainForm mainForm = new MainForm(newGame, selectedDifficulty);
                mainForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing game: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        private void btnQuit_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error quitting application: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads a saved game state using the player's initials.
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string playerInitials = txtInitials.Text.Trim();

                if (string.IsNullOrEmpty(playerInitials))
                {
                    MessageBox.Show("Enter your initials to load a game.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate initials.
                if (!playerInitials.All(char.IsLetter))
                {
                    MessageBox.Show("Initials must contain only letters.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string loadPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "NumberCruncherGame",
                    $"gamestate_{playerInitials}.json"
                );

                if (!File.Exists(loadPath))
                {
                    MessageBox.Show($"No saved game for initials: {playerInitials}.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string jsonData = File.ReadAllText(loadPath);
                var loadedGame = JsonSerializer.Deserialize<NumberCruncherGame>(jsonData);

                if (loadedGame != null)
                {
                    MainForm mainForm = new MainForm(loadedGame, loadedGame.Difficulty);
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Loaded game is null. Please check the save file.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load game: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
