using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin.SupplierWindow;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.SupplierCommand.SupplierCard
{
    public class DeleteSupplierButton : ICommand
    {
        SupplierVM VM;

        public DeleteSupplierButton(SupplierVM VM)
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
            var supplier = parameter as Supplier;
            if (supplier != null)
            {
                return DatabaseHelper.CanDeleteSupplier(supplier.Id);
            }
            return false;
        }

        public void Execute(object parameter)
        {
            var supplier = parameter as Supplier;
            VM.ObservableSupplier.Remove(supplier);
            VM.suppliers.Remove(supplier);

            DatabaseHelper.DeleteSupplier(supplier.Id);
        }
    }
}
