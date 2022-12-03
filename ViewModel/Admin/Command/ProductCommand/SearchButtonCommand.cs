using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ConvenienceStore.ViewModel.Admin.AdminVM;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand
{
    public class SearchButtonCommand : ICommand
    {
        InputInfoVM VM;

        public SearchButtonCommand(InputInfoVM VM)
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
            return (!string.IsNullOrEmpty(VM.SearchContent));
        }

        public void Execute(object parameter)
        {
            VM.SetProductsCorrespondSearch(); ;
        }
    }
}
