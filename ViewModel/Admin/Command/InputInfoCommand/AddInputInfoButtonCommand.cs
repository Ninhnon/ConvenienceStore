using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.InputInfoWindow;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand
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
