using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand
{
    public class OpenInputInfoCommand : ICommand
    {
        InputInfoVM VM;

        public OpenInputInfoCommand(InputInfoVM VM)
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
            InputInfo inputInfo = parameter as InputInfo;
            VM.SelectedInputInfo = inputInfo;

            VM.LoadProducts();
        }
    }
}
