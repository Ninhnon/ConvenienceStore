using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin;
using ConvenienceStore.Views.Admin.SubViews;
using ConvenienceStore.Views.Staff;
using ConvenienceStore.Views.Staff.PaymentWindow;
using ConvenienceStore.Views.Staff.TroubleWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public partial class MainStaffViewModel : BaseViewModel
    {
        public static User? StaffCurrent { get; set; }
        public ICommand? EmployeeCommand { get; set; }
        public ICommand? PaymentCommand { get; set; }
        public ICommand? HistoryCommand { get; set; }
        public ICommand? ProfileCommand { get; set; }
        public ICommand? ProductCommand { get; set; }
        public ICommand? VoucherCommand { get; set; }
        public ICommand? ReportCommand { get; set; }
        public ICommand? ShowPanelCommand { get; set; }
        public ICommand? HidePanelCommand { get; set; }
        public ICommand? CloseWindowCommand { get; set; }
        public ICommand? MinimizeWindowCommand { get; set; }
        public ICommand? MouseMoveWindowCommand { get; set; }
        public ICommand? MaximizeWindowCommand { get; set; }
        public ICommand? SupplierCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public static Grid MaskName { get; set; }

        #region commands
        public ICommand CloseMainStaffWindowCM { get; set; }
        public ICommand MinimizeMainStaffWindowCM { get; set; }
        public ICommand MouseMoveWindowCM { get; set; }
        public ICommand LoadMovieScheduleWindow { get; set; }
        public ICommand LoadFoodPageCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand SelectedGenreCM { get; set; }
        public ICommand SelectedDateCM { get; set; }
        public ICommand LoadErrorPageCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand ChangeRoleCM { get; set; }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged(); }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged(); }
        }

        private bool isAdmin;
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; OnPropertyChanged(); }
        }


        #endregion
        private string? _ten;
        public string? Ten
        {
            get => _ten;
            set
            {
                _ten = value; OnPropertyChanged();
            }
        }
        private string? _Email;
        public string? Email
        {
            get => _Email;
            set
            {
                _Email = value; OnPropertyChanged();
            }
        }
        private byte[] _Anh;
        public byte[] Anh
        {
            get => _Anh;
            set
            {
                _Anh = value; OnPropertyChanged();
            }
        }
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        public List<User> danhsach = new();

        public MainStaffViewModel()
        {
            Ten = CurrentAccount.Name;
            //danhsach = DatabaseHelper.FetchingUserData();
            //List= new ObservableCollection<User>(danhsach);
            Anh = DatabaseHelper.LoadAvatar(CurrentAccount.idAccount);
            Email = CurrentAccount.Email;
            //Anh = List.First(x => x.UserName == StaffCurrent.UserName).Image;
            PaymentCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new PaymentWindow();
            });
            HistoryCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new HistoryWindow();

            });
            ProfileCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ProfileWindow();


            });
            ProductCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ProductView();
            });
            ReportCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new TroublePage();

            });
            CloseWindowCommand = new RelayCommand<Grid>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window w = Window.GetWindow(p);
                w.Close();
            });
            LoadCommand = new RelayCommand<StaffMainWindow>(parameter => true, parameter => { parameter.DataContext = new MainStaffViewModel(); });
            SettingCommand = new RelayCommand<StaffMainWindow>(parameter => true, parameter => Set(parameter));



            MinimizeWindowCommand = new RelayCommand<Grid>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window w = Window.GetWindow(p);
                if (w.WindowState != WindowState.Minimized)
                    w.WindowState = WindowState.Minimized;
                else
                    w.WindowState = WindowState.Normal;
            });
            MaximizeWindowCommand = new RelayCommand<Grid>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window w = Window.GetWindow(p);
                if (w.WindowState != WindowState.Maximized)
                    w.WindowState = WindowState.Maximized;
                else
                    w.WindowState = WindowState.Normal;
            });
            MouseMoveWindowCommand = new RelayCommand<Grid>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window w = Window.GetWindow(p);
                w.DragMove();
            }
           );
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });

        }

        public void Set(StaffMainWindow parameter)
        {
            Setting s = new Setting();

            s.nameTxtbox.textBox.Text = CurrentAccount.Name;
            s.emailTxtbox.textBox.Text = CurrentAccount.Email;
            s.phoneTxtbox.textBox.Text = CurrentAccount.Phone;
            s.addressTxtbox.textBox.Text = CurrentAccount.Address;
            s.ImageProduct.ImageSource = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(CurrentAccount.idAccount));
            s.ShowDialog();
            Ten = CurrentAccount.Name;
            Anh = DatabaseHelper.LoadAvatar(CurrentAccount.idAccount);
            Email = CurrentAccount.Email;
        }

        public BitmapImage ConvertByteToBitmapImage(Byte[] image)
        {
            BitmapImage bi = new BitmapImage();
            MemoryStream stream = new MemoryStream();
            if (image == null)
            {
                return null;
            }
            stream.Write(image, 0, image.Length);
            stream.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }





    }
}
//LoadShowtimeDataCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
// {
//     p.SelectedIndex = -1;
//     await LoadShowtimeData();
// });
//LoadErrorPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
// {
//     p.Content = new TroublePage();
// });
//SignoutCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
//{
//    p.Hide();
//    LoginWindow w1 = new LoginWindow();
//    w1.ShowDialog();
//    p.Close();
//});

//ChangeRoleCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
//{
//    p.Hide();
//    MainAdminWindow w1 = new MainAdminWindow();
//    MainAdminViewModel.currentStaff = CurrentStaff;
//    w1.CurrentUserName.Content = CurrentStaff.Name;
//    w1.Show();
//    p.Close();
//});