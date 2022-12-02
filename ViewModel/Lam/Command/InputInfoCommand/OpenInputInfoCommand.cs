using ConvenienceStore.Model;
using ConvenienceStore.Model.Lam;
using ConvenienceStore.Views.Lam.InputInfoWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Lam.Command.InputInfoCommand
{
    public class OpenInputInfoCommand : ICommand
    {
        InputInfoVM VM;

        public OpenInputInfoCommand(InputInfoVM VM)
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
            InputInfo inputInfo = parameter as InputInfo;
            VM.SelectedInputInfo = inputInfo;

            VM.LoadProducts();
        }
    }
}
