using ConvenienceStore.Model;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.Views.Admin.SubViews;
using System.Windows.Forms;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.SubViewModel
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        public ICommand BackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public AddEmployeeViewModel()
        {
            BackCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Back(parameter));
            SaveCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Save(parameter));
        }


        public void Save(AddEmployeeView parameter)
        {
            if(string.IsNullOrEmpty(parameter.nameTxtbox.textBox.Text))
            {
                parameter.nameTxtbox.textBox.Text = "";
                parameter.nameTxtbox.textBox.Focus();
                return;
            }
            Account acc = new Account("0",
                                     parameter.nameTxtbox.textBox.Text.ToString(),
                                     parameter.addressTxtbox.textBox.Text.ToString(),
                                     parameter.phoneTxtbox.textBox.Text.ToString(),
                                     parameter.emailTxtbox.textBox.Text.ToString(),
                                     parameter.usernameTxtbox.textBox.Text.ToString(),
                                     parameter.passwordTxtbox.textBox.Text.ToString());
            AccountDAL.Instance.AddIntoDataBase(acc);
            MessageBox.Show("them thanh cong");



        }
        public void Back(AddEmployeeView parameter)
        {
            parameter.Close();
        }
    }
}
