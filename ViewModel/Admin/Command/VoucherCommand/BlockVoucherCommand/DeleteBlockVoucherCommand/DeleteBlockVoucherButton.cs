using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
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

            for (int i = 0; i < VM.ObservableBlockVouchers.Count; ++i)
            {
                if (blockVoucher.Id == VM.ObservableBlockVouchers[i].Id)
                {
                    VM.ObservableBlockVouchers.RemoveAt(i);
                    VM.blockVouchers.RemoveAt(i);
                    break;
                }
            }

            VM.VoucherSnackbar.MessageQueue?.Enqueue($"Đã xóa Voucher \"{blockVoucher.ReleaseName}\"", null, null, null, false, true, TimeSpan.FromSeconds(0.7));

            DatabaseHelper.DeleteBlockVoucher(blockVoucher.Id);
            (parameter as Window).Close();
        }
    }
}
