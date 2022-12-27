using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using ZXing.Windows.Compatibility;

namespace ConvenienceStore.Views.Staff.ProductWindow
{
    /// <summary>
    /// Interaction logic for BarCodeUC.xaml
    /// </summary>
    public partial class BarCodeUC : UserControl
    {
        //C:\Đại học\Trực quan project\Ninhnew\ConvenienceStore\beep.wav
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
              
                if (result != null)
                {

                  
                    txtBarcode.Text = result.ToString();
                  
                



                }
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
