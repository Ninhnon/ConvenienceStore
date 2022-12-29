using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand
{
    class FilterAllVoucherCommand : ICommand
    {
        VoucherVM VM;

        public FilterAllVoucherCommand(VoucherVM VM)
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
            for (int i = 0; i < VM.vouchers.Count; i++)
            {
                if (VM.vouchers[i].Status == 1)
                {
                    VM.ObservableVouchers.Add(VM.vouchers[i]);
                }
            }
        }
    }
}
