using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.ProductWindow;
using ConvenienceStore.Views.Admin.SupplierWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.SupplierCommand.SupplierCard
{
    public class EditSupplierButton : ICommand
    {
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
            var currentSupplier = (Supplier)parameter;

            var copyCurrentSupplier = new Supplier()
            {
                Name = currentSupplier.Name,
                Address = currentSupplier.Address,
                Email = currentSupplier.Email,
                Phone = currentSupplier.Phone,
            };
            var editSupplierWindow = new EditSupplierWindow()
            {
                DataContext = currentSupplier,
            };

            editSupplierWindow.ShowDialog();

            if (currentSupplier.Name != copyCurrentSupplier.Name ||
                currentSupplier.Address != copyCurrentSupplier.Address ||
                currentSupplier.Email != copyCurrentSupplier.Email ||
                currentSupplier.Phone != copyCurrentSupplier.Phone)
            {
                // Sau khi cửa sổ Edit đóng thì "currentSupplier" đã được update
                DatabaseHelper.UpdateSupplier(currentSupplier);
            }
        }
    }
}
