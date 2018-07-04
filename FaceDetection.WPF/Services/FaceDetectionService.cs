using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.WPF.ViewModels;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FaceDetection.WPF.Services
{
    public class FaceDetectionService : BaseViewModel, IFaceDetectionService
    {
        private CascadeClassifier _cascadeClassifier;
        private VideoCapture _capture;
        private DispatcherTimer _timer;

        public FaceDetectionService()
        {
            _cascadeClassifier = new CascadeClassifier(@"haarcascade_frontalface_alt_tree.xml");
            _capture = new VideoCapture();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += Timer_Tick;
        }

        public event EventHandler ImageSourceChanged;

        protected virtual void OnImageSourceChanged(EventArgs e)
        {
            ImageSourceChanged?.Invoke(this, e);
        }

        public System.Windows.Media.ImageSource ImageSource { get; set; }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            using (Image<Bgr, Byte> imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imageFrame != null)
                {
                    Image<Gray, Byte> grayframe = imageFrame.Convert<Gray, byte>();
                    Rectangle[] faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, System.Drawing.Size.Empty);

                    foreach (Rectangle face in faces)
                        imageFrame.Draw(face, new Bgr(Color.Red), 3);

                    Bitmap bitmap = imageFrame.Bitmap;

                    ImageSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    bitmap.Dispose();
                }
            }
        }
    }
}
