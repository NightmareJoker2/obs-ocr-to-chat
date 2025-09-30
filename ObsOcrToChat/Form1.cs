using ObsOcrToChat.Properties;
using OBSWebsocketDotNet;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using TwitchLib.Client;

namespace ObsOcrToChat
{
    public partial class Form1 : Form
    {
        Bot bot;
        OBSWebsocket obs;
        Thread ocrThread;
        public Form1()
        {
            InitializeComponent();
            StartUp();
        }

        private async void StartUp()
        {
            AuthenticateButton.Enabled = false;
            AuthenticateButton.Text = "Loading...";
            bot = new Bot();
            await Task.Delay(1200);
            if (!string.IsNullOrEmpty(bot.userName))
            {
                AuthenticateButton.Text = bot.userName;
            }
            else
            {
                AuthenticateButton.Text = "Connect";
            }
            AuthenticateButton.Enabled = true;

            obsWebsocketUrl.Text = !string.IsNullOrEmpty(Settings.Default.ObsWebsocketUrl) ? Settings.Default.ObsWebsocketUrl : "ws://127.0.0.1:4455/";
            obsWebsocketPassword.Text = !string.IsNullOrEmpty(Settings.Default.ObsWebsocketPassword) ? Settings.Default.ObsWebsocketPassword : "";

            obs = new OBSWebsocket();

            ocrThread = new Thread(OcrLoop);
            ocrThread.Start();
        }

        private void AuthenticateButton_MouseEnter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bot.userName))
            {
                AuthenticateButton.Text = "Disconnect";
            }
        }

        private void AuthenticateButton_MouseLeave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bot.userName))
            {
                AuthenticateButton.Text = bot.userName;
            }
        }

        private async void AuthenticateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bot.userName))
            {
                AuthenticateButton.Enabled = false;
                AuthenticateButton.Text = "Authenticating...";
                await Task.Run(() => bot.Authenticate());
                await Task.Run(async () =>
                {
                    while (string.IsNullOrEmpty(bot.userName))
                    {
                        await Task.Delay(200);
                    }
                    if (!string.IsNullOrEmpty(bot.userName))
                    {
                        BeginInvoke((MethodInvoker)delegate
                        {
                            AuthenticateButton.Text = bot.userName;
                            AuthenticateButton.Enabled = true;
                        });
                    }
                });
            }
            else
            {
                AuthenticateButton.Enabled = false;
                AuthenticateButton.Text = "Disconnecting...";
                Settings.Default.AccessToken = null;
                if (bot.isConnected)
                {
                    bot.Disconnect();
                }
                AuthenticateButton.Text = "Connect";
                AuthenticateButton.Enabled = true;
            }
        }

        private void OcrLoop()
        {
            obs.ConnectAsync(obsWebsocketUrl.Text, obsWebsocketPassword.Text);
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    if (obs.IsConnected)
                    {
                        string screenshot = obs.GetSourceScreenshot("image.png", "png").Replace("data:image/png;base64,", "");
                        Rectangle cropRect = new Rectangle(1554, 863, 303, 91);
                        MemoryStream pngStream = new MemoryStream(Convert.FromBase64String(screenshot));
                        using (Bitmap src = Image.FromStream(pngStream) as Bitmap)
                        {
                            using (Bitmap target = new Bitmap(cropRect.Width, cropRect.Height))
                            {
                                using (Graphics g = Graphics.FromImage(target))
                                {
                                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                                    pngStream = new MemoryStream();
                                    target.Save(pngStream, System.Drawing.Imaging.ImageFormat.Bmp);
                                    BeginInvoke((MethodInvoker)delegate
                                    {
                                        picturePreview.Image = Image.FromStream(pngStream);
                                    });
                                }
                            }
                        }
                        string ocrResult;
                        using (TesseractEngine engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                        {
                            using (Pix img = Pix.LoadFromMemory(pngStream.ToArray()))
                            {
                                using (Page page = engine.Process(img))
                                {
                                    ocrResult = page.GetText();
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(ocrResult))
                        {
                            ocrResult = ocrResult.Trim();
                            if (PagerTextbox.Text.Trim() != ocrResult)
                            {
                                BeginInvoke((MethodInvoker)delegate
                                {
                                    PagerTextbox.Text = ocrResult;
                                });
                                if (bot.isConnected)
                                {
                                    bot.Send(string.Concat("📟 Detected Message: ", ocrResult, " 📟"));
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Concat(e.GetType(), ": ", e.Message, Environment.NewLine, Environment.NewLine, e.StackTrace));
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // stays open for some reason after this... weird
            System.Environment.Exit(0);
        }

        private void githubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "https://github.com/NightmareJoker2/obs-ocr-to-chat/", UseShellExecute = true });
        }

        private void obsSaveButton_Click(object sender, EventArgs e)
        {
            obs.Disconnect();
            Settings.Default.ObsWebsocketUrl = obsWebsocketUrl.Text;
            Settings.Default.ObsWebsocketPassword = obsWebsocketPassword.Text;
            Settings.Default.Save();
            obs.ConnectAsync(obsWebsocketUrl.Text, obsWebsocketPassword.Text);
        }
    }
}
