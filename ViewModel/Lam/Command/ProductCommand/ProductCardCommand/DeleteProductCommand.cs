using ConvenienceStore.Model.Lam;
using ConvenienceStore.ViewModel.Lam.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Lam.Command.ProductCommand.ProductCardCommand
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
            VM.ObservableProducts.Remove(product);
            VM.products.Remove(product);
            DatabaseHelper.DeleteProduct(product.InputInfoId, product.Barcode);
        }
    }
}
