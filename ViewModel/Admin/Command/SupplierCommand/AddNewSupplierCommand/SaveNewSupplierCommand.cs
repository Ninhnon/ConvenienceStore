using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Utils.Validation;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.SupplierWindow;
using FluentValidation;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.SupplierCommand.AddNewSupplierCommand
{
    public class SaveNewSupplierCommand : ICommand
    {
        SupplierVM VM;

        public SaveNewSupplierCommand(SupplierVM VM)
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
            var window = parameter as AddNewSupplierWindow;

            window.NameErrorMessage.Text = string.Empty;
            window.AddressErrorMessage.Text = string.Empty;
            window.PhoneErrorMessage.Text = string.Empty;

            var newSupplier = new Supplier()
            {
                Number = VM.suppliers.Count + 1,
                Name = window.SupplierName.Text,
                Address = window.Address.Text,
                Email = window.Email.Text,
                Phone = window.Phone.Text,
            };

            var validator = new SupplierValidator();
            validator.RuleLevelCascadeMode = CascadeMode.Stop;

            var results = validator.Validate(newSupplier);

            if (results.IsValid == false)
            {
                foreach (var error in results.Errors)
                {
                    if (error.PropertyName == "Name")
                    {
                        window.NameErrorMessage.Text = error.ErrorMessage;
                    }

                    if (error.PropertyName == "Address")
                    {
                        window.AddressErrorMessage.Text = error.ErrorMessage;
                    }

                    if (error.PropertyName == "Phone")
                    {
                        window.PhoneErrorMessage.Text = error.ErrorMessage;
                    }
                }

                return;
            }

            DatabaseHelper.InsertSupplier(newSupplier);

            newSupplier.Id = DatabaseHelper.NewestSupplierId();

            if (newSupplier.Name.ToLower().Contains(VM.SearchContent.ToLower()))
            {
                VM.ObservableSupplier.Add(newSupplier);
            }
            VM.suppliers.Add(newSupplier);

            VM.SupplierSnackbar.MessageQueue?.Enqueue($"Đã thêm Nhà cung cấp \"{newSupplier.Name}\"", null, null, null, false, true, TimeSpan.FromSeconds(0.7));

            window.Close();
        }
    }
}
