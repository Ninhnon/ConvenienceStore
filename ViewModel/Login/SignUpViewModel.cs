////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using System.Windows;
////using System.Windows.Controls;
////using System.Windows.Input;
////using ConvenienceStore.Model;
////using ConvenienceStore.Utils.DataLayerAccess;
////using ConvenienceStore.ViewModel.Admin;
////using ConvenienceStore.Views;
////using ConvenienceStore.Views.Login;

////namespace ConvenienceStore.ViewModel.Login
////{
////    public class SignUpViewModel : BaseViewModel
////    {
////        //NÚT ĐĂNG KÝ
////        public ICommand SignUpCommand { get; set; }
////        public ICommand PasswordChangedCommand { get; set; }
////        public ICommand PasswordConfirmChangedCommand { get; set; }
////        private bool isSignUp_;
////        public bool IsSignUp { get => isSignUp_; set => isSignUp_ = value; }
////        private string password_;
////        public string Password
////        {
////            get => password_;
////            set { password_ = value; OnPropertyChanged(); }
////        }
////        private string username_;
////        public string Username
////        {
////            get => username_;
////            set { username_ = value; OnPropertyChanged(); }
////        }
////        private string passwordConfirm_;
////        public string PasswordConfirm
////        {
////            get => passwordConfirm_;
////            set
////            {
////                passwordConfirm_ = value; OnPropertyChanged();
////            }

////        }
////        private int type_;
////        public int Type
////        {
////            get => type_;
////            set
////            {
////                type_ = value; OnPropertyChanged();
////            }
////        }
////        public void SignUp(SignUpWindow parameter)
////        {

////            isSignUp_ = false;
////            if (Password != PasswordConfirm)
////            {
////                MessageBox.Show("mat khau khong trung khop");
////                return;
////            }
////            int idAccount = AccountDAL.Instance.SetNewID();
////            if (idAccount != -1)
////            {
////                Account newAccount = new Account();
////                AccountDAL.Instance.AddIntoDataBase(newAccount);


////                MessageBox.Show("Đăng ký thành công!");
////                isSignUp_ = true;

////                parameter.txtUsername.Text = null;
////                parameter.pwbPassword.Password = "";
////                parameter.pwbPasswordConfirm.Password = "";

////            }

////        }
////        public void EncodePassword(PasswordBox parameter)
////        {
////            Password = parameter.Password;
////            Password = MD5Hash(Password);
////        }
////        public void EncodePasswordConfirm(PasswordBox parameter)
////        {
////            PasswordConfirm = parameter.Password;
////            PasswordConfirm = MD5Hash(PasswordConfirm);
////        }
////        public SignUpViewModel()
////        {
////            SignUpCommand = new RelayCommand<SignUpWindow>((parameter) => true, (parameter) => SignUp(parameter));
////            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => true, (parameter) => EncodePassword(parameter));
////            PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>((paramter) => true, (parameter) => EncodePasswordConfirm(parameter));
////        }
////        //NÚT ĐĂNG KÝ

////    }
////}
