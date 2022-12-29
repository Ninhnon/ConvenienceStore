using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin.ProductWindow;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.ProductCardCommand
{
    public class EditProductButton : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            var currentProduct = (Product)values[0];
            var snackbar = (Snackbar)values[1];

            var coppyCurProduct = new Product()
            {
                Title = currentProduct.Title,
                Type = currentProduct.Type,
                Image = currentProduct.Image,
                Cost = currentProduct.Cost,
                Price = currentProduct.Price,
                ManufacturingDate = currentProduct.ManufacturingDate,
                ExpiryDate = currentProduct.ExpiryDate,
                Discount = currentProduct.Discount
            };

            // currentProduct là 1 biến tham thiếu tới product thật nằm trong "products" ở ProductsVM.
            // sửa giá trị trên currentProduct thì giá trị Product thật cũng thay đổi theo !!
            // Nên copy ra 1 biến mới để không ảnh hướng đến Product thật

            EditProductWindow editProductWindow = new EditProductWindow();

            editProductWindow.DataContext = currentProduct;
            switch (currentProduct.Type)
            {
                case "Đồ ăn":
                    editProductWindow.TypeComboBox.SelectedIndex = 0;
                    break;
                case "Thức uống":
                    editProductWindow.TypeComboBox.SelectedIndex = 1;
                    break;
                case "Khác":
                    editProductWindow.TypeComboBox.SelectedIndex = 2;
                    break;
            }

            editProductWindow.ShowDialog();

            if (currentProduct.Title != coppyCurProduct.Title ||
                currentProduct.Type != coppyCurProduct.Type ||
                currentProduct.Image != coppyCurProduct.Image ||
                currentProduct.Cost != coppyCurProduct.Cost ||
                currentProduct.Price != coppyCurProduct.Price ||
                currentProduct.ManufacturingDate != coppyCurProduct.ManufacturingDate ||
                currentProduct.ExpiryDate != coppyCurProduct.ExpiryDate ||
                currentProduct.Discount != coppyCurProduct.Discount)
            {
                // Sau khi cửa sổ Edit đóng thì "currentProduct" đã được update
                // 1. Thông báo edit thành công
                snackbar.MessageQueue?.Enqueue($"Đã cập nhật Lô sản phẩm \"{currentProduct.Title}\"", null, null, null, false, true, TimeSpan.FromSeconds(0.8));
                // 2. Update to DB
                DatabaseHelper.UpdateProduct(currentProduct);
            }
        }
    }
}
