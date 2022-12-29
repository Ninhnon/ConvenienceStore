using System;
using System.Windows;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command
{
    public class CloseAppCommand : ICommand
    {
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
            var window = parameter as Window;
            window.Close();
        }
    }
}
