using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.SmallProductCommand
{
    public class RefreshSmallProductData : ICommand
    {
        ProductVM VM;
        public RefreshSmallProductData(ProductVM VM)
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
            VM.smallProducts = DatabaseHelper.FetchingSmallProductData();

            VM.ObservableSmallProducts.Clear();
            for (int i = 0; i < VM.smallProducts.Count; i++)
            {
                VM.ObservableSmallProducts.Add(VM.smallProducts[i]);
            }
        }
    }
}
