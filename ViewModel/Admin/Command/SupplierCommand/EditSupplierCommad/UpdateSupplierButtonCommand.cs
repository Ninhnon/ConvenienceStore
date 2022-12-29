using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Validation;
using ConvenienceStore.Views.Admin.SupplierWindow;
using FluentValidation;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.SupplierCommand.EditSupplierCommad
{
    public class UpdateSupplierButtonCommand : ICommand
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
            var window = parameter as EditSupplierWindow;

            window.NameErrorMessage.Text = string.Empty;
            window.AddressErrorMessage.Text = string.Empty;
            window.PhoneErrorMessage.Text = string.Empty;

            var newSupplier = new Supplier()
            {
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

            var curSupplier = window.DataContext as Supplier;

            curSupplier.Name = newSupplier.Name;
            curSupplier.Address = newSupplier.Address;
            curSupplier.Email = newSupplier.Email;
            curSupplier.Phone = newSupplier.Phone;

            window.Close();
        }
    }
}
