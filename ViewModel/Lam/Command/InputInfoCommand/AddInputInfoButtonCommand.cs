using ConvenienceStore.Model.Lam;
using ConvenienceStore.Views.Lam.InputInfoWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Lam.Command.InputInfoCommand
{
    public class AddInputInfoButtonCommand : ICommand
    {
        InputInfoVM VM;

        public AddInputInfoButtonCommand(InputInfoVM VM)
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
            AddNewInputInfoWindow addNewInputInfoWindow = new AddNewInputInfoWindow();
            addNewInputInfoWindow.DataContext = VM;
            addNewInputInfoWindow.ShowDialog();
        }
    }
}
