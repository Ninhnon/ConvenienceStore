using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin.InputInfoWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ConvenienceStore.Model;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand
{
    public class CreateInputInfoButtonCommand : ICommand
    {
        InputInfoVM VM;

        public CreateInputInfoButtonCommand(InputInfoVM VM)
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
            var window = parameter as AddNewInputInfoWindow;
            window.ComboBoxSupplierErrorMessage.Text = string.Empty;

            var supplier = (Supplier)window.SupplierComboBox.SelectedItem;

            if (supplier == null)
            {
                window.ComboBoxSupplierErrorMessage.Text = "Chưa chọn nhà cung cấp";
                return;
            }

            var newInputInfo = new InputInfo()
            {
                InputDate = (DateTime)window.DatePicker.SelectedDate,
                UserId = 2,
                UserName = "Trần Lê Hoàng Lâm",
                SupplierName = supplier.Name,
            };

            DatabaseHelper.InsertInputInfo(newInputInfo.InputDate, 2, supplier.Id);

            newInputInfo.Id = DatabaseHelper.NewestInputInfoId();

            VM.inputInfos.Add(newInputInfo);

            if (VM.SelectedManager.Id == newInputInfo.UserId || VM.SelectedManager.Name == "All")
            {
                VM.ObservableInputInfos.Add(newInputInfo);
            }
            
            
            window.Close();
        }
    }
}
