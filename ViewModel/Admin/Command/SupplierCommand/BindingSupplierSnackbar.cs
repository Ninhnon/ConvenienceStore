using ConvenienceStore.ViewModel.Admin.AdminVM;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.SupplierCommand
{
    public class BindingSupplierSnackbar : ICommand
    {
        SupplierVM VM;
        public BindingSupplierSnackbar(SupplierVM VM)
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
            VM.SupplierSnackbar = parameter as Snackbar;
        }
    }
}
