using ConvenienceStore.Model.Staff;
using ConvenienceStore.Views.Login;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConvenienceStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Staff : Window
    {
        User user;
        public Staff()
        {
            InitializeComponent();
        }
        public Staff(User tk)
        {
            this.user = tk;
            //this.MaNV = tk.MaNV;
            InitializeComponent();
        }
        private void ListViewItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_product.Visibility = Visibility.Collapsed;
                tt_voucher.Visibility = Visibility.Collapsed;
                tt_report.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_product.Visibility = Visibility.Visible;
                tt_voucher.Visibility = Visibility.Visible;
                tt_report.Visibility = Visibility.Visible;
            }
        }

        private void click_ThayDoiAnh(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new()
            {
                Title = "Select an image",
                Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png"
            };
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //xử lý đổi tên file truyền vào
                string[] arr = openFile.FileName.Split('\\');
                string[] arrFileName = arr[arr.Length - 1].Split('.');
                string newNameFile = "NV" + "-" + DateTime.Now.Ticks.ToString() + "." + arrFileName[arrFileName.Length - 1];

                try
                {
                    string sourceFile = openFile.FileName;
                    string targetPath = Environment.CurrentDirectory + "\\Res";
                    //Combine file và đường dẫn
                    string destFile = System.IO.Path.Combine(targetPath, newNameFile);

                    //Copy file từ file nguồn đến file đích
                    File.Copy(sourceFile, destFile, true);

                    //gán ngược lại giao diện
                    Uri uri = new Uri(destFile);
                    ImageBrush imageBrush = new ImageBrush(new BitmapImage(uri));
                    imgAvatar.Fill = imageBrush;
                    //Thêm đường dẫn vào DB

                }
                catch (Exception ex) { }

            }


        }

        private void btnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
