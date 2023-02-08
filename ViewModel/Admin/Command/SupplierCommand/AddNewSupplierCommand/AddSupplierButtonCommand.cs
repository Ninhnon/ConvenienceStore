using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.SupplierWindow;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.SupplierCommand.AddNewSupplierCommand
{
    public class AddSupplierButtonCommand : ICommand
    {
        SupplierVM VM;

        public AddSupplierButtonCommand(SupplierVM VM)
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
            AddNewSupplierWindow addNewSupplierWindow = new AddNewSupplierWindow();
            addNewSupplierWindow.DataContext = VM;
            addNewSupplierWindow.ShowDialog();
        }
    }
}
