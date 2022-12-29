using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.InputInfoWindow;
using System;
using System.Windows.Input;

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
                UserId = CurrentAccount.idAccount,
                UserName = CurrentAccount.Name,
                Email = CurrentAccount.Email,
                Phone = CurrentAccount.Phone,
                Avatar = CurrentAccount.Avatar,
                SupplierName = supplier.Name,
            };

            DatabaseHelper.InsertInputInfo(newInputInfo.InputDate, CurrentAccount.idAccount, supplier.Id);

            newInputInfo.Id = DatabaseHelper.NewestInputInfoId();

            if (VM.SelectedManager.Id == newInputInfo.UserId || VM.SelectedManager.Name == "Tất cả")
            {
                VM.ObservableInputInfos.Add(newInputInfo);
            }
            VM.inputInfos.Add(newInputInfo);

            VM.InputInfoSnackbar.MessageQueue?.Enqueue($"Đã tạo Đợt nhập hàng ngày {newInputInfo.InputDate.ToString("dd/MM/yyyy")}", null, null, null, false, true, TimeSpan.FromSeconds(0.9));

            window.Close();
        }
    }
}
