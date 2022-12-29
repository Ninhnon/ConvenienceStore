using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.AddNewProductCommand
{
    public class CheckExistBarcode : ICommand
    {
        InputInfoVM VM;

        public CheckExistBarcode(InputInfoVM VM)
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
            var values = (object[])parameter;
            var s = (string)values[0];
            return 8 <= s.Length && s.Length <= 13;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            var s = (string)values[0];
            var barcodeErrorMessage = (TextBlock)values[1];
            for (int i = 0; i < VM.SelectedInputInfo.products.Count; ++i)
            {
                if (s == VM.SelectedInputInfo.products[i].Barcode)
                {
                    barcodeErrorMessage.Text = "Mã sản phẩm này đã được nhập";
                    return;
                }
            }
            barcodeErrorMessage.Text = "";
        }
    }
}
