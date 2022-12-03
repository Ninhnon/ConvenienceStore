using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Views.Admin;
using ConvenienceStore.Views.Staff;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Login
{
    public class LoginViewModelNinh : BaseViewModel
    {
        public bool IsLogin { get; set; }
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public ICommand CloseCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        public List<User> danhsach = new();

        public LoginViewModelNinh()
        {

            danhsach = DatabaseHelper.FetchingUserData();
            List = new ObservableCollection<User>(danhsach);
            IsLogin = false;
            Password = "";
            UserName = "";
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });

        }

        void Login(Window p)
        {
            if (p == null)
                return;

            /*
             admin
             admin

            staff
            staff
             */
            IsLogin = true;
            //string passEncode = MD5Hash(Base64Encode(Password));

            var accCount = List.Where(x => x.UserName == UserName && x.Password == Password/*passEncode*/).Count();
            var role = List.First(x => x.UserName == UserName && x.Password == Password).UserRole;
            var acc = List.First(x => x.UserName == UserName && x.Password == Password);
            var accimg = List.First(x => x.UserName == UserName && x.Password == Password).Image;
            if (accCount > 0)
            {
                IsLogin = true;
                if (role == "0")
                {
                    //MainStaffViewModel.StaffCurrent = acc;
                    //MainStaffViewModel.Image = acc.Image;
                    StaffMainWindow n = new StaffMainWindow();

                    //n.txbHoTenNV.Text = acc.Name;

                    //string s = Convert.ToBase64String(acc.Image);

                    //BitmapImage imageSource = new BitmapImage();

                    //imageSource.BeginInit();
                    //imageSource.StreamSource = new MemoryStream(System.Convert.FromBase64String(s));
                    //imageSource.EndInit();
                    //ImageBrush imageBrush = new ImageBrush(imageSource);
                    //n.imgAvatar.Fill = imageBrush;
                    //n.imgAvatar.ImageSource = image;
                    n.Show();
                    p.Close();
                }
                if (role == "1")
                {
                    //MainStaffViewModel.StaffCurrent = acc;
                    //MainStaffViewModel.Image = acc.Image;
                    AdminMainWindow n = new AdminMainWindow();

                    //n.txbHoTenNV.Text = acc.Name;

                    //string s = Convert.ToBase64String(acc.Image);

                    //BitmapImage imageSource = new BitmapImage();

                    //imageSource.BeginInit();
                    //imageSource.StreamSource = new MemoryStream(System.Convert.FromBase64String(s));
                    //imageSource.EndInit();
                    //ImageBrush imageBrush = new ImageBrush(imageSource);
                    //n.imgAvatar.Fill = imageBrush;
                    //n.imgAvatar.ImageSource = image;
                    n.Show();
                    p.Close();
                }
            }
            else
            {
                IsLogin = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
        }

        //public static string Base64Encode(string plainText)
        //{
        //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        //    return System.Convert.ToBase64String(plainTextBytes);
        //}

        //public static string MD5Hash(string input)
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}


    }
}
