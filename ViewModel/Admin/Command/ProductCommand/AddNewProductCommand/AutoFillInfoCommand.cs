using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin.ProductWindow;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.AddNewProductCommand
{
    public class AutoFillInfoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            var window = parameter as AddNewProductWindow;
            var BarcodeContent = window.BarcodeTextBox.Text;
            if (!string.IsNullOrEmpty(BarcodeContent))
            {
                return BarcodeContent.Length >= 8;  // Update
            }
            return false;
        }

        public void Execute(object parameter)
        {
            // Feature:
            // Khi manager nhập đúng barcode của sản phẩm đã tồn tại trong DB
            // Thì tự fill 2 thông tin có sẵn là Title + Image

            var window = parameter as AddNewProductWindow;
            var BarcodeContent = window.BarcodeTextBox.Text;

            string title;
            byte[] bytes;
            string type;

            (title, bytes, type) = DatabaseHelper.FetchingProductTableViaBarcode(BarcodeContent);

            if (bytes == null) return;  // Update

            string s = Convert.ToBase64String(bytes);

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.StreamSource = new MemoryStream(System.Convert.FromBase64String(s));
            bi.EndInit();

            window.TitleTextBox.Text = title;
            window.ImageProduct.ImageSource = bi;

            switch (type)
            {
                case "Đồ ăn":
                    window.TypeComboBox.SelectedIndex = 0;
                    break;
                case "Thức uống":
                    window.TypeComboBox.SelectedIndex = 1;
                    break;
                case "Khác":
                    window.TypeComboBox.SelectedIndex = 2;
                    break;
            }
        }
    }
}
