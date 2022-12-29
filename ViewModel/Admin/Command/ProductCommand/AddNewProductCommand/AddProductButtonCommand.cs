using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.ProductWindow;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.AddNewProductCommand
{
    public class AddProductButtonCommand : ICommand
    {
        InputInfoVM VM;

        public AddProductButtonCommand(InputInfoVM VM)
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
            AddNewProductWindow addNewProductWindow = new AddNewProductWindow();
            addNewProductWindow.DataContext = VM;
            addNewProductWindow.ShowDialog();
        }
    }
}
