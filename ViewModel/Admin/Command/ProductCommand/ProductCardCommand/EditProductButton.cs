using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin.ProductWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.ProductCardCommand
{
    public class EditProductButton : ICommand
    {
        InputInfoVM VM;

        public EditProductButton(InputInfoVM VM)
        {
            this.VM = VM;
        }

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
            var currentProduct = (Product)parameter;

            // currentProduct là 1 biến tham thiếu tới product thật nằm trong "products" ở ProductsVM.
            // sửa giá trị trên currentProduct thì giá trị Product thật cũng thay đổi theo !!
            // Nên copy ra 1 biến mới để không ảnh hướng đến Product thậ

            EditProductWindow editProductWindow = new EditProductWindow();

            editProductWindow.DataContext = currentProduct;


            editProductWindow.ShowDialog();
            // Sau khi cửa sổ Edit đóng thì "currentProduct" đã được update
            VM.SetProductsCorrespondSearch();


            // Update to Database
            DatabaseHelper.Update(currentProduct);
        }
    }
}
