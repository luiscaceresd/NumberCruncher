using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

    }
}
