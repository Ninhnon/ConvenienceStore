using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.Views.Admin.SubViews;
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
