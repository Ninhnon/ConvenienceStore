using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin.SupplierWindow;
using MaterialDesignThemes.Wpf;
using System;
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
            var values = (object[])parameter;
            var currentSupplier = (Supplier)values[0];
            var snackbar = (Snackbar)values[1];

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
                snackbar.MessageQueue?.Enqueue($"Đã cập nhật Nhà cung cấp \"{currentSupplier.Name}\"", null, null, null, false, true, TimeSpan.FromSeconds(0.8));
                DatabaseHelper.UpdateSupplier(currentSupplier);
            }
        }
    }
}
