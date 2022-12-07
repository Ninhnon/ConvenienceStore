using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.Views.Admin.SubViews;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<Account> observableEmployee_;
        public ObservableCollection<Account> ObservableEmployee
        {
            get { return observableEmployee_; }
            set { observableEmployee_ = value;
                OnPropertyChanged();
            }
        }



        public ICommand AddEmployeeCommand { get; set; }

        public EmployeeViewModel()
        {
            List<Account> accounts = AccountDAL.Instance.ConvertDataTableToList();
            AddEmployeeCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => AddEmployee(parameter));
            ObservableEmployee = new ObservableCollection<Account>();
            for (int i = 0; i < accounts.Count; i++)
            {
                ObservableEmployee.Add(accounts[i]);
            }
        }
        public void AddEmployee(AddEmployeeView parameter)
        {
            AddEmployeeView addView = new AddEmployeeView();
           
            addView.ShowDialog();
        }
    }
}
