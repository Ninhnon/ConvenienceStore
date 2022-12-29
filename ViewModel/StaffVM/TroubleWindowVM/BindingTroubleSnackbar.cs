using ConvenienceStore.ViewModel.TroubleWindowVM;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.StaffVM.TroubleWindowVM
{
    public class BindingTroubleSnackbar : ICommand
    {
        TroublePageViewModel VM;
        public BindingTroubleSnackbar(TroublePageViewModel VM)
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
            VM.TroubleSnackbar = parameter as Snackbar;
        }
    }
}
