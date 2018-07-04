using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FaceDetection.WPF.Controls
{
    public class EmguImageBox : System.Windows.Controls.Image
    {
        private CascadeClassifier _cascadeClassifier;
        private VideoCapture _capture;
        private DispatcherTimer _timer;
        private Stopwatch _stopWatch;

        public EmguImageBox()
        {
            _capture = new VideoCapture();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += Timer_Tick;

            _stopWatch = new Stopwatch();

            Loaded += EmguImageBox_Loaded;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (e.ClickCount > 1 && !_timer.IsEnabled)
                _timer.Start();
        }

        private void EmguImageBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(CascadeClassifierFileName))
                _cascadeClassifier = new CascadeClassifier(CascadeClassifierFileName);

            Start();

            Loaded -= EmguImageBox_Loaded;
        }

        public string CascadeClassifierFileName
        {
            get { return (string)GetValue(CascadeClassifierFileNameProperty); }
            set { SetValue(CascadeClassifierFileNameProperty, value); }
        }

        public static readonly DependencyProperty CascadeClassifierFileNameProperty =
            DependencyProperty.Register(nameof(CascadeClassifierFileName), typeof(string), typeof(EmguImageBox), new PropertyMetadata(null, OnCascadeClassifierFileNameChanged));

        public byte[] FaceDetected
        {
            get { return (byte[])GetValue(FaceDetectedProperty); }
            set { SetValue(FaceDetectedProperty, value); }
        }

        public static readonly DependencyProperty FaceDetectedProperty =
            DependencyProperty.Register(nameof(FaceDetected), typeof(byte[]), typeof(EmguImageBox), new PropertyMetadata(null));

        private static void OnCascadeClassifierFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is EmguImageBox image) || e.OldValue == e.NewValue)
                return;

            image.OnCascadeClassifierFileNameChanged(e);
        }

        private void OnCascadeClassifierFileNameChanged(DependencyPropertyChangedEventArgs e)
        {
            Stop();

            string fileName = e.NewValue?.ToString();
            _cascadeClassifier = String.IsNullOrEmpty(fileName) ? null : new CascadeClassifier(fileName);

            Start();
        }

        public void Start()
        {
            if (_cascadeClassifier == null)
                return;

            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_cascadeClassifier == null)
                return;

            using (Image<Bgr, Byte> imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imageFrame != null)
                {
                    Image<Gray, Byte> grayframe = imageFrame.Convert<Gray, byte>();
                    Rectangle[] faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, System.Drawing.Size.Empty);

                    if (!faces.Any() && _stopWatch.IsRunning)
                        _stopWatch.Stop();

                    foreach (Rectangle face in faces)
                    {
                        imageFrame.Draw(face, new Bgr(Color.Red), 3);

                        if (_stopWatch.IsRunning)
                        {
                            if (_stopWatch.ElapsedMilliseconds > 1000)
                            {
                                _stopWatch.Stop();
                                Stop();
                                FaceDetected = imageFrame.ToJpegData();
                            }
                        }
                        else
                        {
                            _stopWatch.Start();
                        }
                    }

                    Source = ToBitmapSource(imageFrame);
                }
            }
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap();

                BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr);
                return bs;
            }
        }
    }
}
