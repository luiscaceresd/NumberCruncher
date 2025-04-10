using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumberCruncherClient
{
    public partial class PlayerSetupForm : Form
    {
        public PlayerSetupForm()
        {
            InitializeComponent();
        }

        private void PlayerSetupForm_Load(object sender, EventArgs e)
        {

        }

        private void btnEasy_Click(object sender, EventArgs e)
        {
            StartGame(Difficulty.EASY);

        }

        private void btnModerate_Click(object sender, EventArgs e)
        {
            StartGame(Difficulty.MODERATE);
        }

        private void btnDifficult_Click(object sender, EventArgs e)
        {
            StartGame(Difficulty.DIFFICULT);
        }
        private void StartGame(Difficulty difficulty)
        {
            string initials = txtInitials.Text;
            if (string.IsNullOrWhiteSpace(initials))
            {
                MessageBox.Show("Please enter your initials before starting the game.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NumberCruncherGame game = new NumberCruncherGame();
            game.Player.setInitials(initials);
            game.Difficulty = difficulty;
            game.startGame();

            // Pass difficulty to the MainScreen
            MainForm mainScreen = new MainForm(game, difficulty);
            mainScreen.Show();


            // Hide the PlayerSetupForm
            this.Hide();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string initials = txtInitials.Text.Trim();

            if(string.IsNullOrEmpty(initials))
            {
                MessageBox.Show("Please enter your initials to load a game.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "NumberCruncherGame",
                    $"gamestate_{initials}.json"
                );
            if (!File.Exists(path))
            {
                MessageBox.Show($"There are no games to Load for Initials : {initials}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var loadedGame = JsonSerializer.Deserialize<NumberCruncherGame>(json);

                if (loadedGame != null)
                {
                    MainForm mainForm = new MainForm(loadedGame, loadedGame.Difficulty);
                    mainForm.Show();
                    this.Hide();
                }
            }
            catch
            {
                MessageBox.Show("Failed to Load the Game. The Save File might be corrupted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
