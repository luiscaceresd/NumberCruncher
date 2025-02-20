namespace NumberCruncherClient
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStartGame = new Button();
            btnSubmitLevel = new Button();
            btnNextLevel = new Button();
            lblInitials = new Label();
            lblDifficulty = new Label();
            txtInitials = new TextBox();
            cmbDifficulty = new ComboBox();
            panelTracks = new Panel();
            lblResult = new Label();
            SuspendLayout();
            // 
            // btnStartGame
            // 
            btnStartGame.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartGame.Location = new Point(10, 153);
            btnStartGame.Margin = new Padding(3, 2, 3, 2);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(155, 48);
            btnStartGame.TabIndex = 0;
            btnStartGame.Text = "Start Game";
            btnStartGame.UseVisualStyleBackColor = true;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // btnSubmitLevel
            // 
            btnSubmitLevel.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubmitLevel.Location = new Point(10, 206);
            btnSubmitLevel.Margin = new Padding(3, 2, 3, 2);
            btnSubmitLevel.Name = "btnSubmitLevel";
            btnSubmitLevel.Size = new Size(155, 48);
            btnSubmitLevel.TabIndex = 1;
            btnSubmitLevel.Text = "Submit Level";
            btnSubmitLevel.UseVisualStyleBackColor = true;
            btnSubmitLevel.Click += btnSubmitLevel_Click;
            // 
            // btnNextLevel
            // 
            btnNextLevel.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNextLevel.Location = new Point(10, 258);
            btnNextLevel.Margin = new Padding(3, 2, 3, 2);
            btnNextLevel.Name = "btnNextLevel";
            btnNextLevel.Size = new Size(155, 48);
            btnNextLevel.TabIndex = 2;
            btnNextLevel.Text = "Next Level";
            btnNextLevel.UseVisualStyleBackColor = true;
            btnNextLevel.Click += btnNextLevel_Click;
            // 
            // lblInitials
            // 
            lblInitials.AutoSize = true;
            lblInitials.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInitials.Location = new Point(10, 29);
            lblInitials.Name = "lblInitials";
            lblInitials.Size = new Size(155, 14);
            lblInitials.TabIndex = 3;
            lblInitials.Text = "Enter your initials:";
            // 
            // lblDifficulty
            // 
            lblDifficulty.AutoSize = true;
            lblDifficulty.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDifficulty.Location = new Point(10, 65);
            lblDifficulty.Name = "lblDifficulty";
            lblDifficulty.Size = new Size(140, 14);
            lblDifficulty.TabIndex = 4;
            lblDifficulty.Text = "Select Difficulty:";
            // 
            // txtInitials
            // 
            txtInitials.Location = new Point(181, 26);
            txtInitials.Margin = new Padding(3, 2, 3, 2);
            txtInitials.Name = "txtInitials";
            txtInitials.Size = new Size(133, 23);
            txtInitials.TabIndex = 5;
            // 
            // cmbDifficulty
            // 
            cmbDifficulty.FormattingEnabled = true;
            cmbDifficulty.Items.AddRange(new object[] { "Easy", "Moderate", "Difficult" });
            cmbDifficulty.Location = new Point(181, 62);
            cmbDifficulty.Margin = new Padding(3, 2, 3, 2);
            cmbDifficulty.Name = "cmbDifficulty";
            cmbDifficulty.Size = new Size(133, 23);
            cmbDifficulty.TabIndex = 6;
            // 
            // panelTracks
            // 
            panelTracks.BorderStyle = BorderStyle.FixedSingle;
            panelTracks.Location = new Point(330, 26);
            panelTracks.Margin = new Padding(3, 2, 3, 2);
            panelTracks.Name = "panelTracks";
            panelTracks.Size = new Size(266, 281);
            panelTracks.TabIndex = 7;
            // 
            // lblResult
            // 
            lblResult.BorderStyle = BorderStyle.FixedSingle;
            lblResult.Location = new Point(614, 26);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(172, 281);
            lblResult.TabIndex = 8;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(806, 338);
            Controls.Add(lblResult);
            Controls.Add(panelTracks);
            Controls.Add(cmbDifficulty);
            Controls.Add(txtInitials);
            Controls.Add(lblDifficulty);
            Controls.Add(lblInitials);
            Controls.Add(btnNextLevel);
            Controls.Add(btnSubmitLevel);
            Controls.Add(btnStartGame);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NumberCruncher";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStartGame;
        private Button btnSubmitLevel;
        private Button btnNextLevel;
        private Label lblInitials;
        private Label lblDifficulty;
        private TextBox txtInitials;
        private ComboBox cmbDifficulty;
        private Panel panelTracks;
        private Label lblResult;
    }
}
