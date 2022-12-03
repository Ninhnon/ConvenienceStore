using System.Windows.Input;
using ConvenienceStore.Views.Admin.SubViews;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class EmployeeViewModel : BaseViewModel
    {
        public ICommand AddEmployeeCommand { get; set; }

        public EmployeeViewModel()
        {
            AddEmployeeCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => AddEmployee(parameter));
        }
        public void AddEmployee(AddEmployeeView parameter)
        {
            AddEmployeeView addView = new AddEmployeeView();
           
            addView.ShowDialog();
        }
    }
}
