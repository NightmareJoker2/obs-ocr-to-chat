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
            obsLabel = new Label();
            obsWebsocketUrl = new TextBox();
            obsWebsocketPassword = new TextBox();
            obsSaveButton = new Button();
            githubLink = new LinkLabel();
            picturePreview = new PictureBox();
            obsSourceSelector = new ComboBox();
            obsSourceLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)picturePreview).BeginInit();
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
            AuthenticateButton.Click += AuthenticateButton_Click;
            AuthenticateButton.MouseEnter += AuthenticateButton_MouseEnter;
            AuthenticateButton.MouseLeave += AuthenticateButton_MouseLeave;
            // 
            // PagerTextbox
            // 
            PagerTextbox.Location = new Point(96, 35);
            PagerTextbox.Multiline = true;
            PagerTextbox.Name = "PagerTextbox";
            PagerTextbox.Size = new Size(187, 79);
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
            // obsLabel
            // 
            obsLabel.AutoSize = true;
            obsLabel.Location = new Point(61, 123);
            obsLabel.Name = "obsLabel";
            obsLabel.Size = new Size(29, 15);
            obsLabel.TabIndex = 3;
            obsLabel.Text = "OBS";
            // 
            // obsWebsocketUrl
            // 
            obsWebsocketUrl.Location = new Point(96, 120);
            obsWebsocketUrl.Name = "obsWebsocketUrl";
            obsWebsocketUrl.PlaceholderText = "ws://127.0.0.1:12345/";
            obsWebsocketUrl.Size = new Size(158, 23);
            obsWebsocketUrl.TabIndex = 4;
            // 
            // obsWebsocketPassword
            // 
            obsWebsocketPassword.Location = new Point(260, 120);
            obsWebsocketPassword.Name = "obsWebsocketPassword";
            obsWebsocketPassword.PasswordChar = '*';
            obsWebsocketPassword.PlaceholderText = "Password";
            obsWebsocketPassword.Size = new Size(148, 23);
            obsWebsocketPassword.TabIndex = 5;
            // 
            // obsSaveButton
            // 
            obsSaveButton.Location = new Point(414, 120);
            obsSaveButton.Name = "obsSaveButton";
            obsSaveButton.Size = new Size(39, 23);
            obsSaveButton.TabIndex = 6;
            obsSaveButton.Text = "Save";
            obsSaveButton.UseVisualStyleBackColor = true;
            obsSaveButton.Click += obsSaveButton_Click;
            // 
            // githubLink
            // 
            githubLink.AutoSize = true;
            githubLink.Location = new Point(153, 337);
            githubLink.Name = "githubLink";
            githubLink.Size = new Size(299, 15);
            githubLink.TabIndex = 7;
            githubLink.TabStop = true;
            githubLink.Text = "https://github.com/NightmareJoker2/obs-ocr-to-chat/";
            githubLink.LinkClicked += githubLink_LinkClicked;
            // 
            // picturePreview
            // 
            picturePreview.Location = new Point(12, 177);
            picturePreview.Name = "picturePreview";
            picturePreview.Size = new Size(438, 157);
            picturePreview.TabIndex = 8;
            picturePreview.TabStop = false;
            // 
            // obsSourceSelector
            // 
            obsSourceSelector.FormattingEnabled = true;
            obsSourceSelector.Location = new Point(96, 148);
            obsSourceSelector.Name = "obsSourceSelector";
            obsSourceSelector.Size = new Size(312, 23);
            obsSourceSelector.TabIndex = 9;
            obsSourceSelector.SelectionChangeCommitted += ObsSourceSelector_SelectionChangeCommitted;
            obsSourceSelector.SelectedIndexChanged += ObsSourceSelector_SelectedIndexChanged;
            // 
            // obsSourceLabel
            // 
            obsSourceLabel.AutoSize = true;
            obsSourceLabel.Location = new Point(47, 151);
            obsSourceLabel.Name = "obsSourceLabel";
            obsSourceLabel.Size = new Size(43, 15);
            obsSourceLabel.TabIndex = 10;
            obsSourceLabel.Text = "Source";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 361);
            Controls.Add(obsSourceLabel);
            Controls.Add(obsSourceSelector);
            Controls.Add(picturePreview);
            Controls.Add(githubLink);
            Controls.Add(obsSaveButton);
            Controls.Add(obsWebsocketPassword);
            Controls.Add(obsWebsocketUrl);
            Controls.Add(obsLabel);
            Controls.Add(PagerLabel);
            Controls.Add(PagerTextbox);
            Controls.Add(AuthenticateButton);
            Name = "Form1";
            Text = "OBS OCR to Chat";
            FormClosed += Form1_FormClosed;
            ((System.ComponentModel.ISupportInitialize)picturePreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button AuthenticateButton;
        private TextBox PagerTextbox;
        private Label PagerLabel;
        private Label obsLabel;
        private TextBox obsWebsocketUrl;
        private TextBox obsWebsocketPassword;
        private Button obsSaveButton;
        private LinkLabel githubLink;
        private PictureBox picturePreview;
        private ComboBox obsSourceSelector;
        private Label obsSourceLabel;
    }
}
