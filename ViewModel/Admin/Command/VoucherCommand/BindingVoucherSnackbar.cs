using ConvenienceStore.ViewModel.Admin.AdminVM;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand
{
    class BindingVoucherSnackbar : ICommand
    {
        VoucherVM VM;
        public BindingVoucherSnackbar(VoucherVM VM)
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
            VM.VoucherSnackbar = parameter as Snackbar;
        }
    }
}
