using ConvenienceStore.Views.SubViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.SubViewModel
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        public ICommand BackCommand { get; set; }

        public AddEmployeeViewModel()
        {
            BackCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Back(parameter));
        }
        public void Back(AddEmployeeView parameter)
        {
            parameter.Close();
        }
    }
}
