using ConvenienceStore.Model;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Admin.SubViews;
using System.Windows.Input;


namespace ConvenienceStore.ViewModel.SubViewModels
{
    public class ChangePasswordViewModel
    {
        public ICommand BackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ChangePasswordViewModel()
        {
            BackCommand = new RelayCommand<ChangePasswordView>(parameter => true, parameter => Back(parameter));
            SaveCommand = new RelayCommand<ChangePasswordView>(parameter => true, parameter => Save(parameter));
        }


        public void Back(ChangePasswordView parameter)
        {
            parameter.Close();
        }

        public void Save(ChangePasswordView parameter)
        {
            bool isValid = true;
            parameter.CurrentPasswordBox.ErrorMessage.Text = "";
            parameter.NewPasswordBox.ErrorMessage.Text = "";
            parameter.ConfirmPasswordBox.ErrorMessage.Text = "";
            if (string.IsNullOrEmpty(parameter.CurrentPasswordBox.passwordBox.Password.ToString()))
            {
                parameter.CurrentPasswordBox.ErrorMessage.Text = "Vui lòng nhập mật khẩu hiện tại";
                parameter.CurrentPasswordBox.passwordBox.Focus();
                isValid = false;
            }
            else if (CurrentAccount.Password != BaseViewModel.MD5Hash(BaseViewModel.MD5Hash(parameter.CurrentPasswordBox.passwordBox.Password.ToString())))
            {
                parameter.CurrentPasswordBox.ErrorMessage.Text = "Mật khẩu hiện tại không chính xác!";
                parameter.CurrentPasswordBox.passwordBox.Focus();
                isValid = false;
            }
            else if (string.IsNullOrEmpty(parameter.NewPasswordBox.passwordBox.Password.ToString()))
            {
                parameter.NewPasswordBox.ErrorMessage.Text = "Vui lòng nhập mật khẩu mới";
                parameter.NewPasswordBox.passwordBox.Focus();
                isValid = false;
            }
            else if (string.IsNullOrEmpty(parameter.ConfirmPasswordBox.passwordBox.Password.ToString()))
            {
                parameter.ConfirmPasswordBox.ErrorMessage.Text = "Vui lòng xác nhận mật khẩu mới";
                parameter.ConfirmPasswordBox.passwordBox.Focus();
                isValid = false;
            }

            else if (parameter.NewPasswordBox.passwordBox.Password.ToString() != parameter.ConfirmPasswordBox.passwordBox.Password.ToString())
            {
                parameter.ConfirmPasswordBox.ErrorMessage.Text = "Mật khẩu xác nhận không chính xác";
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            if (isValid)
            {

                AccountDAL.Instance.UpdatePassword(BaseViewModel.MD5Hash(BaseViewModel.MD5Hash(parameter.NewPasswordBox.passwordBox.Password.ToString())), CurrentAccount.Email);
                MessageBoxCustom mb = new("Thông báo", "Thay đổi mật khẩu thành công", MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();
            }


        }





    }
}
