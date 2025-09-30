using OBSWebsocketDotNet;
using System.Threading.Tasks;
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
            await Task.Delay(500);
            if (!string.IsNullOrEmpty(bot.userName))
            {
                AuthenticateButton.Text = bot.userName;
            }
            else
            {
                AuthenticateButton.Text = "Connect";
            }
            AuthenticateButton.Enabled = true;

            obs = new OBSWebsocket();

            ocrThread = new Thread(OcrLoop);
            ocrThread.Start();
        }

        private async void AuthenticateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bot.userName))
            {
                await Task.Run(() => bot.Authenticate());
                if (!string.IsNullOrEmpty(bot.userName))
                {
                    await Task.Delay(500);
                    BeginInvoke((MethodInvoker)delegate
                    {
                        AuthenticateButton.Text = bot.userName;
                    });
                }
            }
        }

        private void OcrLoop()
        {
            obs.ConnectAsync("ws://127.0.0.1:4455/", "");
            while (true)
            {
                Thread.Sleep(1000);
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
    }
}
