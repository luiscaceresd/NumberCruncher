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
            btnStartGame.Location = new Point(12, 204);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(177, 64);
            btnStartGame.TabIndex = 0;
            btnStartGame.Text = "Start Game";
            btnStartGame.UseVisualStyleBackColor = true;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // btnSubmitLevel
            // 
            btnSubmitLevel.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubmitLevel.Location = new Point(12, 274);
            btnSubmitLevel.Name = "btnSubmitLevel";
            btnSubmitLevel.Size = new Size(177, 64);
            btnSubmitLevel.TabIndex = 1;
            btnSubmitLevel.Text = "Submit Level";
            btnSubmitLevel.UseVisualStyleBackColor = true;
            btnSubmitLevel.Click += btnSubmitLevel_Click;
            // 
            // btnNextLevel
            // 
            btnNextLevel.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNextLevel.Location = new Point(12, 344);
            btnNextLevel.Name = "btnNextLevel";
            btnNextLevel.Size = new Size(177, 64);
            btnNextLevel.TabIndex = 2;
            btnNextLevel.Text = "Next Level";
            btnNextLevel.UseVisualStyleBackColor = true;
            btnNextLevel.Click += btnNextLevel_Click;
            // 
            // lblInitials
            // 
            lblInitials.AutoSize = true;
            lblInitials.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInitials.Location = new Point(12, 39);
            lblInitials.Name = "lblInitials";
            lblInitials.Size = new Size(189, 18);
            lblInitials.TabIndex = 3;
            lblInitials.Text = "Enter your initials:";
            // 
            // lblDifficulty
            // 
            lblDifficulty.AutoSize = true;
            lblDifficulty.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDifficulty.Location = new Point(12, 87);
            lblDifficulty.Name = "lblDifficulty";
            lblDifficulty.Size = new Size(170, 18);
            lblDifficulty.TabIndex = 4;
            lblDifficulty.Text = "Select Difficulty:";
            // 
            // txtInitials
            // 
            txtInitials.Location = new Point(207, 34);
            txtInitials.Name = "txtInitials";
            txtInitials.Size = new Size(151, 27);
            txtInitials.TabIndex = 5;
            // 
            // cmbDifficulty
            // 
            cmbDifficulty.FormattingEnabled = true;
            cmbDifficulty.Items.AddRange(new object[] { "Easy", "Moderate", "Difficult" });
            cmbDifficulty.Location = new Point(207, 82);
            cmbDifficulty.Name = "cmbDifficulty";
            cmbDifficulty.Size = new Size(151, 28);
            cmbDifficulty.TabIndex = 6;
            // 
            // panelTracks
            // 
            panelTracks.BorderStyle = BorderStyle.FixedSingle;
            panelTracks.Location = new Point(377, 34);
            panelTracks.Name = "panelTracks";
            panelTracks.Size = new Size(304, 374);
            panelTracks.TabIndex = 7;
            // 
            // lblResult
            // 
            lblResult.BorderStyle = BorderStyle.FixedSingle;
            lblResult.Location = new Point(702, 34);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(196, 374);
            lblResult.TabIndex = 8;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(921, 450);
            Controls.Add(lblResult);
            Controls.Add(panelTracks);
            Controls.Add(cmbDifficulty);
            Controls.Add(txtInitials);
            Controls.Add(lblDifficulty);
            Controls.Add(lblInitials);
            Controls.Add(btnNextLevel);
            Controls.Add(btnSubmitLevel);
            Controls.Add(btnStartGame);
            Name = "MainForm";
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
