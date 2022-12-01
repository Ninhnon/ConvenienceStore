using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ConvenienceStore.Views;
using ConvenienceStore.Views.SubViews;
using MaterialDesignThemes.Wpf;

namespace ConvenienceStore.ViewModel
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
