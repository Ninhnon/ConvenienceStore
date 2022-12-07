using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand.DeleteInputInfoCommand
{
    public class DeleteInputInfo : ICommand
    {
        InputInfoVM VM;
        public DeleteInputInfo(InputInfoVM VM)
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
            var inputInfo = VM.SelectedInputInfo;  // inputInfo cần delete

            VM.ObservableInputInfos.Remove(inputInfo);
            VM.inputInfos.Remove(inputInfo);
            DatabaseHelper.DeleteInputInfo(inputInfo.Id);

            (parameter as Window).Close();
        }
    }
}
