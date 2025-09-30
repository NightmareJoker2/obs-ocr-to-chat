namespace ObsOcrToChat
{
    partial class Form1
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
            AuthenticateButton = new Button();
            PagerTextbox = new TextBox();
            PagerLabel = new Label();
            SuspendLayout();
            // 
            // AuthenticateButton
            // 
            AuthenticateButton.Location = new Point(289, 12);
            AuthenticateButton.Name = "AuthenticateButton";
            AuthenticateButton.Size = new Size(161, 23);
            AuthenticateButton.TabIndex = 0;
            AuthenticateButton.Text = "Authenticate";
            AuthenticateButton.UseVisualStyleBackColor = true;
            AuthenticateButton.MouseEnter += AuthenticateButton_MouseEnter;
            AuthenticateButton.MouseLeave += AuthenticateButton_MouseLeave;
            AuthenticateButton.Click += AuthenticateButton_Click;
            // 
            // PagerTextbox
            // 
            PagerTextbox.Location = new Point(96, 35);
            PagerTextbox.Name = "PagerTextbox";
            PagerTextbox.Size = new Size(184, 23);
            PagerTextbox.TabIndex = 1;
            // 
            // PagerLabel
            // 
            PagerLabel.AutoSize = true;
            PagerLabel.Location = new Point(12, 38);
            PagerLabel.Name = "PagerLabel";
            PagerLabel.Size = new Size(78, 15);
            PagerLabel.TabIndex = 2;
            PagerLabel.Text = "Detected Text";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 361);
            Controls.Add(PagerLabel);
            Controls.Add(PagerTextbox);
            Controls.Add(AuthenticateButton);
            Name = "Form1";
            Text = "OBS OCR to Chat";
            ResumeLayout(false);
            PerformLayout();
            FormClosed += Form1_FormClosed;
        }

        #endregion

        private Button AuthenticateButton;
        private TextBox PagerTextbox;
        private Label PagerLabel;
    }
}
