using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand
{
    class PrefixCodeLostFocus : ICommand
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
            var textBox = (TextBox)parameter;
            textBox.Text = textBox.Text.ToUpper();
        }
    }
}
