using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZXing.Windows.Compatibility;

namespace ConvenienceStore.Views.Staff.PaymentWindow
{
    /// <summary>
    /// Interaction logic for BarCodeUC.xaml
    /// </summary>
    public partial class BarCodeUC : UserControl
    {
        private string result1 { get; set; }
        private int count = 0;
        SoundPlayer player = new SoundPlayer(@"..\..\..\beep.wav");
        public FilterInfoCollection filterInfoCollection;
        public VideoCaptureDevice videoCaptureDevice;
        public BarCodeUC()
        {
            InitializeComponent();
            FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filterInfoCollection)
            {
                cboCamera.Items.Add(device.Name);
            }
            cboCamera.SelectedIndex = 0;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
            btnStart.Visibility = Visibility.Collapsed;
        }

        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            this.Dispatcher.Invoke(() =>
            {
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
                BarcodeReader reader = new BarcodeReader();

                var result = reader.Decode(bitmap);
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                }
                pictureBox.Source = bitmapImage;
                if (result != null)
                {
                    result1 = result.ToString();
                    Thread.Sleep(33);




                    count += 2;
                }
                if (count >= 5)

                {
                    txtBarcode.Text = "";


                    txtBarcode.Text = result.ToString();
                    player.Play();


                    count = -20;
                }






            });



        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.SignalToStop();
                }
            }
            this.Visibility = Visibility.Hidden;
            btnStart.Visibility = Visibility.Visible;
        }
    }
}
