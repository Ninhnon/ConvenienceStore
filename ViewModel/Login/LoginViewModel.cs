using ConvenienceStore.Model;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Admin;
using ConvenienceStore.Views.Login;
using ConvenienceStore.Views.Staff;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Login
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand OpenForgotPasswordWindowCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }
        private bool isLogin;
        public bool IsLogin { get => isLogin; set => isLogin = value; }

        public LoginViewModel()
        {
            LogInCommand = new RelayCommand<LoginWindow>((parameter) => true, (parameter) => Login(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => true, (parameter) => EncodingPassword(parameter));
            OpenForgotPasswordWindowCommand = new RelayCommand<System.Windows.Window>((parameter) => true, (parameter) => OpenForgotPasswordWindow(parameter));
        }
        public void Login(LoginWindow parameter)
        {
            isLogin = false;
            List<Account> accounts = AccountDAL.Instance.ConvertDataTableToList();
            if (string.IsNullOrEmpty(parameter.txtUsername.Text))
            {
                MessageBoxCustom mb = new("Cảnh báo", "Hãy nhập tài khoản", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                parameter.txtUsername.Focus();
                return;
            }
            if (string.IsNullOrEmpty(parameter.txtPassword.Password.ToString()))

            {
                MessageBoxCustom mb = new("Cảnh báo", "Hãy nhập mật khẩu", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                parameter.txtPassword.Focus();
                return;
            }
            int flag = 0;
            foreach (var account in accounts)
            {

                if (account.UserName == UserName && account.Password == Password)
                {
                    CurrentAccount.UserRole = account.UserRole;
                    CurrentAccount.Name = account.Name;
                    CurrentAccount.Email = account.Email;
                    CurrentAccount.Address = account.Address;
                    CurrentAccount.Phone = account.Phone;
                    CurrentAccount.idAccount = account.IdAccount;
                    CurrentAccount.Password = account.Password;
                    CurrentAccount.Avatar = account.Avatar;
                    CurrentAccount.ManagerId = account.ManagerId;

                    MessageBoxCustom mb = new("Thông báo", "Đăng nhập thành công", MessageType.Success, MessageButtons.OK);
                    flag = 1;
                    isLogin = true;
                    mb.ShowDialog();
                    //mb = new("Thông báo", "Đăng nhập thất bại", MessageType.Error, MessageButtons.OK);
                    //mb.ShowDialog();
                    //mb = new("Thông báo", "Bạn có muốn xóa hay không?", MessageType.Warning, MessageButtons.YesNo);
                    //mb.ShowDialog();
                    //mb = new("Thông báo", "Sự cố đã được thêm từ trước", MessageType.Info, MessageButtons.OK);
                    //mb.ShowDialog();
                    break;

                }

            }
            if (flag == 0)
            {
                MessageBoxCustom mb = new("Cảnh báo", "Tên đăng nhập hoặc mật khẩu không đúng", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                parameter.txtUsername.Focus();
                parameter.txtPassword.Focus();
            }
            if (isLogin == true && CurrentAccount.UserRole == "1")
            {
                AdminMainWindow home = new AdminMainWindow();

                parameter.Close();

                home.Dispatcher.Invoke(home.ShowDialog);



            }
            else if (isLogin == true && CurrentAccount.UserRole == "0")
            {
                StaffMainWindow home = new StaffMainWindow();

                parameter.Close();
                home.Dispatcher.Invoke(home.ShowDialog);



            }
        }

        public void EncodingPassword(PasswordBox parameter)
        {
            password = parameter.Password;
            this.password = MD5Hash(this.password);
            this.password = MD5Hash(this.password);
        }
        public void OpenForgotPasswordWindow(System.Windows.Window parameter)
        {
            ForgotPasswordWindow forgot = new ForgotPasswordWindow();


            parameter.WindowStyle = WindowStyle.None;
            forgot.ShowDialog();

            parameter.Opacity = 1;
            parameter.Show();
        }

    }
}
