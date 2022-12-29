using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.ProductCardCommand
{
    public class DeleteProductCommand : ICommand
    {
        InputInfoVM VM;
        public DeleteProductCommand(InputInfoVM VM)
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
            var product = parameter as Product;

            VM.ProductSnackbar.MessageQueue?.Enqueue($"Đã xóa Lô sản phẩm \"{product.Title}\"", null, null, null, false, true, TimeSpan.FromSeconds(0.7));

            VM.ObservableProducts.Remove(product);
            VM.products.Remove(product);

            DatabaseHelper.DeleteProduct(product.InputInfoId, product.Barcode);
        }
    }
}
