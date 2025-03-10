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
            SuspendLayout();
            // 
            // txtInitials
            // 
            txtInitials.Location = new Point(64, 70);
            txtInitials.Margin = new Padding(3, 2, 3, 2);
            txtInitials.Name = "txtInitials";
            txtInitials.Size = new Size(133, 23);
            txtInitials.TabIndex = 7;
            // 
            // lblInitials
            // 
            lblInitials.AutoSize = true;
            lblInitials.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInitials.Location = new Point(45, 35);
            lblInitials.Name = "lblInitials";
            lblInitials.Size = new Size(155, 14);
            lblInitials.TabIndex = 6;
            lblInitials.Text = "Enter your initials:";
            // 
            // lblDifficulty
            // 
            lblDifficulty.AutoSize = true;
            lblDifficulty.Font = new Font("Stencil", 9F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblDifficulty.Location = new Point(346, 35);
            lblDifficulty.Name = "lblDifficulty";
            lblDifficulty.Size = new Size(135, 14);
            lblDifficulty.TabIndex = 8;
            lblDifficulty.Text = "Select Difficulty";
            // 
            // btnEasy
            // 
            btnEasy.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEasy.Location = new Point(351, 74);
            btnEasy.Margin = new Padding(3, 2, 3, 2);
            btnEasy.Name = "btnEasy";
            btnEasy.Size = new Size(134, 52);
            btnEasy.TabIndex = 9;
            btnEasy.Text = "Easy";
            btnEasy.UseVisualStyleBackColor = true;
            btnEasy.Click += btnEasy_Click;
            // 
            // btnModerate
            // 
            btnModerate.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnModerate.Location = new Point(351, 130);
            btnModerate.Margin = new Padding(3, 2, 3, 2);
            btnModerate.Name = "btnModerate";
            btnModerate.Size = new Size(134, 52);
            btnModerate.TabIndex = 10;
            btnModerate.Text = "Moderate";
            btnModerate.UseVisualStyleBackColor = true;
            btnModerate.Click += btnModerate_Click;
            // 
            // btnDifficult
            // 
            btnDifficult.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDifficult.Location = new Point(351, 188);
            btnDifficult.Margin = new Padding(3, 2, 3, 2);
            btnDifficult.Name = "btnDifficult";
            btnDifficult.Size = new Size(134, 52);
            btnDifficult.TabIndex = 11;
            btnDifficult.Text = "Difficult";
            btnDifficult.UseVisualStyleBackColor = true;
            btnDifficult.Click += btnDifficult_Click;
            // 
            // PlayerSetupForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(545, 299);
            Controls.Add(btnDifficult);
            Controls.Add(btnModerate);
            Controls.Add(btnEasy);
            Controls.Add(lblDifficulty);
            Controls.Add(txtInitials);
            Controls.Add(lblInitials);
            Margin = new Padding(3, 2, 3, 2);
            Name = "PlayerSetupForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PlayerSetupForm";
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
    }
}