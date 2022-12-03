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

            var maxId = VM.inputInfos.Max(p => p.Id);

            var newInputInfo = new InputInfo()
            {
                Id = maxId + 1,
                InputDate = (DateTime)window.DatePicker.SelectedDate,
                UserName = "Trần Lê Hoàng Lâm",
                SupplierName = "Công ty HCT",
            };

            VM.inputInfos.Add(newInputInfo);
            DatabaseHelper.InsertInputInfo(newInputInfo.InputDate, 2, 1);

            window.Close();
        }
    }
}
