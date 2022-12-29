using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.VoucherWindow;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand.DeleteBlockVoucherCommand
{
    class OpenAlertDialog : ICommand
    {
        VoucherVM VM;
        public OpenAlertDialog(VoucherVM VM)
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
            VM.DeletedBlockVoucher = parameter as BlockVoucher;
            var alertDialog = new AlertDialogDeleteBlockVoucher()
            {
                DataContext = VM,
            };
            alertDialog.ShowDialog();
        }
    }
}
