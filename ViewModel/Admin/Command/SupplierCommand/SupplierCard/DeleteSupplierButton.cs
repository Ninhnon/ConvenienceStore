using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
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
            if (parameter != null && (int)parameter - 1 < VM.suppliers.Count)
            {
                return DatabaseHelper.CanDeleteSupplier(VM.suppliers[(int)parameter - 1].Id);
            }
            return false;
        }

        public void Execute(object parameter)
        {
            var indexOfSupplier = (int)parameter - 1;
            VM.SupplierSnackbar.MessageQueue?.Enqueue($"Đã xóa Nhà cung cấp \"{VM.suppliers[indexOfSupplier].Name}\"", null, null, null, false, true, TimeSpan.FromSeconds(0.7));

            DatabaseHelper.DeleteSupplier(VM.suppliers[indexOfSupplier].Id);

            VM.ObservableSupplier.RemoveAt(indexOfSupplier);
            VM.suppliers.RemoveAt(indexOfSupplier);

            for (int i = indexOfSupplier; i < VM.suppliers.Count; ++i)
            {
                VM.suppliers[i].Number = i + 1;
            }


        }
    }
}
