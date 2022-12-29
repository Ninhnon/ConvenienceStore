using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand
{
    class HideDeleteButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            Border border = (Border)parameter;
            return !border.IsMouseOver;
        }

        public void Execute(object parameter)
        {
            Border border = (Border)parameter;
            border.Visibility = Visibility.Hidden;
        }
    }
}
