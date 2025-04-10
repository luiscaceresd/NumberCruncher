namespace NumberCruncherClient
{
    partial class PlayerSetupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtInitials = new TextBox();
            lblInitials = new Label();
            lblDifficulty = new Label();
            btnEasy = new Button();
            btnModerate = new Button();
            btnDifficult = new Button();
            btnQuit = new Button();
            btnLoad = new Button();
            SuspendLayout();
            // 
            // txtInitials
            // 
            txtInitials.Location = new Point(73, 99);
            txtInitials.Name = "txtInitials";
            txtInitials.Size = new Size(151, 27);
            txtInitials.TabIndex = 0;
            // 
            // lblInitials
            // 
            lblInitials.AutoSize = true;
            lblInitials.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInitials.Location = new Point(51, 47);
            lblInitials.Name = "lblInitials";
            lblInitials.Size = new Size(189, 18);
            lblInitials.TabIndex = 6;
            lblInitials.Text = "Enter your initials:";
            // 
            // lblDifficulty
            // 
            lblDifficulty.AutoSize = true;
            lblDifficulty.Font = new Font("Stencil", 9F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblDifficulty.Location = new Point(395, 47);
            lblDifficulty.Name = "lblDifficulty";
            lblDifficulty.Size = new Size(164, 18);
            lblDifficulty.TabIndex = 8;
            lblDifficulty.Text = "Select Difficulty";
            // 
            // btnEasy
            // 
            btnEasy.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEasy.Location = new Point(401, 99);
            btnEasy.Name = "btnEasy";
            btnEasy.Size = new Size(153, 69);
            btnEasy.TabIndex = 1;
            btnEasy.Text = "Easy";
            btnEasy.UseVisualStyleBackColor = true;
            btnEasy.Click += btnEasy_Click;
            // 
            // btnModerate
            // 
            btnModerate.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnModerate.Location = new Point(401, 173);
            btnModerate.Name = "btnModerate";
            btnModerate.Size = new Size(153, 69);
            btnModerate.TabIndex = 2;
            btnModerate.Text = "Moderate";
            btnModerate.UseVisualStyleBackColor = true;
            btnModerate.Click += btnModerate_Click;
            // 
            // btnDifficult
            // 
            btnDifficult.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDifficult.Location = new Point(401, 251);
            btnDifficult.Name = "btnDifficult";
            btnDifficult.Size = new Size(153, 69);
            btnDifficult.TabIndex = 3;
            btnDifficult.Text = "Difficult";
            btnDifficult.UseVisualStyleBackColor = true;
            btnDifficult.Click += btnDifficult_Click;
            // 
            // btnQuit
            // 
            btnQuit.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuit.Location = new Point(73, 287);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(151, 33);
            btnQuit.TabIndex = 4;
            btnQuit.Text = "Quit";
            btnQuit.UseVisualStyleBackColor = true;
            btnQuit.Click += btnQuit_Click;
            // 
            // btnLoad
            // 
            btnLoad.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoad.Location = new Point(73, 251);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(151, 30);
            btnLoad.TabIndex = 9;
            btnLoad.Text = "Resume";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // PlayerSetupForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(623, 399);
            Controls.Add(btnLoad);
            Controls.Add(btnQuit);
            Controls.Add(btnDifficult);
            Controls.Add(btnModerate);
            Controls.Add(btnEasy);
            Controls.Add(lblDifficulty);
            Controls.Add(txtInitials);
            Controls.Add(lblInitials);
            Name = "PlayerSetupForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Player Setup";
            Load += PlayerSetupForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtInitials;
        private Label lblInitials;
        private Label lblDifficulty;
        private Button btnEasy;
        private Button btnModerate;
        private Button btnDifficult;
        private Button btnQuit;
        private Button btnLoad;
    }
}