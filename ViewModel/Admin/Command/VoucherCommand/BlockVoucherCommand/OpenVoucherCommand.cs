using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand
{
    class OpenVoucherCommand : ICommand
    {
        VoucherVM VM;

        public OpenVoucherCommand(VoucherVM VM)
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
            BlockVoucher blockVoucher = parameter as BlockVoucher;
            VM.SelectedBlockVoucher = blockVoucher;

            VM.LoadVouchers();
        }
    }
}
