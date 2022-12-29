using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand
{
    class FilterActiveVoucherCommand : ICommand
    {
        VoucherVM VM;

        public FilterActiveVoucherCommand(VoucherVM VM)
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
            if (VM.SelectedBlockVoucher != null)
            {
                for (int i = VM.ObservableVouchers.Count - 1; i >= 0; i--)
                {
                    if (VM.ObservableVouchers[i].Status == 1)
                    {
                        VM.ObservableVouchers.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
