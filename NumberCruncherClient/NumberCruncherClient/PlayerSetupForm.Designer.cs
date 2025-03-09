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
            txtInitials.Location = new Point(73, 93);
            txtInitials.Name = "txtInitials";
            txtInitials.Size = new Size(151, 27);
            txtInitials.TabIndex = 7;
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
            lblDifficulty.Location = new Point(396, 47);
            lblDifficulty.Name = "lblDifficulty";
            lblDifficulty.Size = new Size(164, 18);
            lblDifficulty.TabIndex = 8;
            lblDifficulty.Text = "Select Difficulty";
            // 
            // btnEasy
            // 
            btnEasy.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEasy.Location = new Point(401, 98);
            btnEasy.Name = "btnEasy";
            btnEasy.Size = new Size(153, 70);
            btnEasy.TabIndex = 9;
            btnEasy.Text = "Easy";
            btnEasy.UseVisualStyleBackColor = true;
            btnEasy.Click += btnEasy_Click;
            // 
            // btnModerate
            // 
            btnModerate.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnModerate.Location = new Point(401, 174);
            btnModerate.Name = "btnModerate";
            btnModerate.Size = new Size(153, 70);
            btnModerate.TabIndex = 10;
            btnModerate.Text = "Moderate";
            btnModerate.UseVisualStyleBackColor = true;
            btnModerate.Click += btnModerate_Click;
            // 
            // btnDifficult
            // 
            btnDifficult.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDifficult.Location = new Point(401, 250);
            btnDifficult.Name = "btnDifficult";
            btnDifficult.Size = new Size(153, 70);
            btnDifficult.TabIndex = 11;
            btnDifficult.Text = "Difficult";
            btnDifficult.UseVisualStyleBackColor = true;
            btnDifficult.Click += btnDifficult_Click;
            // 
            // PlayerSetupForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(623, 399);
            Controls.Add(btnDifficult);
            Controls.Add(btnModerate);
            Controls.Add(btnEasy);
            Controls.Add(lblDifficulty);
            Controls.Add(txtInitials);
            Controls.Add(lblInitials);
            Name = "PlayerSetupForm";
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