using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand.DeleteInputInfoCommand
{
    public class DeleteInputInfoCommand : ICommand
    {
        InputInfoVM VM;
        public DeleteInputInfoCommand(InputInfoVM VM)
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
            var inputInfo = VM.DeletedInputInfo;  // inputInfo cần delete

            VM.ObservableInputInfos.Remove(inputInfo);
            VM.inputInfos.Remove(inputInfo);

            VM.InputInfoSnackbar.MessageQueue?.Enqueue($"Đã xóa Đợt nhập hàng ngày {inputInfo.InputDate.ToString("dd/MM/yyyy")}", null, null, null, false, true, TimeSpan.FromSeconds(0.7));

            DatabaseHelper.DeleteInputInfo(inputInfo.Id);
            (parameter as Window).Close();
        }
    }
}
