using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenkAlgılama
{
    public partial class mainForm : Form
    {
        private WebcamHandler webcamHandler;

        public mainForm()
        {
            InitializeComponent();
            webcamHandler = new WebcamHandler();
            webcamHandler.NewFrameReceived += WebcamHandler_NewFrameReceived;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblWebcam.Text = "Webcam :";
            lblRGB.Text = $"R: {0}, G: {0}, B: {0}";

            if (!webcamHandler.StartCapturing())
            {
                MessageBox.Show("Video cihazı bulunamadı.");
                Close();
            }
        }
        private void WebcamHandler_NewFrameReceived(Bitmap frame)
        {
            if (pbMain.InvokeRequired)
            {
                pbMain.Invoke((Action)(() => pbMain.Image = frame));
            }
            else
            {
                pbMain.Image = frame;
            }
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            webcamHandler.StopCapturing();
        }

        private void pbMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (pbMain.Image != null)
            {
                Bitmap bmp = new Bitmap(pbMain.Image);

                int x = e.X;
                int y = e.Y;

                if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                {
                    Color pixelColor = bmp.GetPixel(x, y);

                    int red = pixelColor.R;
                    int green = pixelColor.G;
                    int blue = pixelColor.B;

                    lblRGB.Text = $"R: {red}, G: {green}, B: {blue}";
                }

                bmp.Dispose();
            }
        }
    }
}
