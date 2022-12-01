using ConvenienceStore.ViewModel.Lam.Helpers;
using ConvenienceStore.Views.Lam.ProductWindow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.Lam.Command.ProductCommand.AddNewProductCommand
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
                return BarcodeContent.Length >= 12;
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

            (title, bytes) = DatabaseHelper.FetchingProductTableViaBarcode(BarcodeContent);

            string s = Convert.ToBase64String(bytes);

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.StreamSource = new MemoryStream(System.Convert.FromBase64String(s));
            bi.EndInit();

            window.TitleTextBox.Text = title;
            window.ImageProduct.ImageSource = bi;
        }
    }
}
