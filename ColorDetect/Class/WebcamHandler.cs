using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using System.Drawing;
using AForge.Video.DirectShow;

namespace RenkAlgılama
{
    public class WebcamHandler
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public event Action<Bitmap> NewFrameReceived;

        public bool StartCapturing()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                return false;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            videoSource.Start();

            return true;
        }

        public void StopCapturing()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            NewFrameReceived?.Invoke((Bitmap)eventArgs.Frame.Clone());
        }
    }
}
