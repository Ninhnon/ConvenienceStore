using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand
{
    public class RefreshInputInfoData : ICommand
    {
        InputInfoVM VM;

        public RefreshInputInfoData(InputInfoVM VM)
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
            VM.inputInfos = DatabaseHelper.FetchingInputInfo();
            VM.ObservableInputInfos.Clear();
            for (int i = 0; i < VM.inputInfos.Count; i++)
            {
                VM.ObservableInputInfos.Add(VM.inputInfos[i]);
            }
        }
    }
}
