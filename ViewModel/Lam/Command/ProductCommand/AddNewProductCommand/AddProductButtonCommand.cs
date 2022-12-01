using ConvenienceStore.Views.Lam.ProductWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Lam.Command.ProductCommand.AddNewProductCommand
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
