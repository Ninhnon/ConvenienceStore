using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.Utils.Converters
{
    public class ImageConverter
    {
        private static ImageConverter instance;

        public static ImageConverter Instance
        {
            get { if (instance == null) instance = new ImageConverter(); return ImageConverter.instance; }
            private set { ImageConverter.instance = value; }
        }
        private ImageConverter()
        {

        }

        public Byte[] ConvertImageToBytes(string imageFileName)
        {
            FileStream fs = new FileStream(imageFileName, FileMode.Open, FileAccess.Read);

            //Initialize a byte array with size of stream
            byte[] imgByteArr = new byte[fs.Length];

            //Read data from the file stream and put into the byte array
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

            //Close a file stream
            fs.Close();
            return imgByteArr;
        }
        public BitmapImage ConvertByteToBitmapImage(Byte[] image)
        {
            BitmapImage bi = new BitmapImage();
            MemoryStream stream = new MemoryStream();
            if (image == null)
            {
                return null;
            }
            stream.Write(image, 0, image.Length);
            stream.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        public byte[] ConvertBitmapImageToBytes(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }


}
