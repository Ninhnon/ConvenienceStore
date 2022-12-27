﻿using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.VoucherCommand
{
    class RefreshData : ICommand
    {
        VoucherVM VM;
        public RefreshData(VoucherVM VM)
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
            VM.blockVouchers = DatabaseHelper.FetchingBlockVoucherData();

            VM.ObservableBlockVouchers = new ObservableCollection<BlockVoucher>();
            for (int i = 0; i < VM.blockVouchers.Count; i++)
            {
                VM.ObservableBlockVouchers.Add(VM.blockVouchers[i]);
            }
        }
    }
}
