using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand.DeleteBlockVoucherCommand
{
    class DeleteBlockVoucherButton : ICommand
    {
        VoucherVM VM;
        public DeleteBlockVoucherButton(VoucherVM VM)
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
            var blockVoucher = VM.DeletedBlockVoucher;

            VM.ObservableBlockVouchers.Remove(blockVoucher);
            VM.blockVouchers.Remove(blockVoucher);

            DatabaseHelper.DeleteBlockVoucher(blockVoucher.Id);
            (parameter as Window).Close();
        }
    }
}
