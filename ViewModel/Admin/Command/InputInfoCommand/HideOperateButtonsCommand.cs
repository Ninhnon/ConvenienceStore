using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand
{
    public class HideOperateButtonsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            Grid grid = (Grid)parameter;
            return !grid.IsMouseOver;
        }

        public void Execute(object parameter)
        {
            Grid grid = (Grid)parameter;
            grid.Visibility = Visibility.Hidden;
        }
    }
}
