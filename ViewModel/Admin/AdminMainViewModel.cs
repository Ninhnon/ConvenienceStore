
using ConvenienceStore.Views.Admin;
using System.Windows.Controls;
using System.Windows.Input;
using ConvenienceStore.Model;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin;
using ConvenienceStore.Views.Admin.SubViews;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.Admin
{
    public class AdminMainViewModel : StaffVM.BaseViewModel
    {
        private double wdHeight_ = 660;
        private double wdWidth_ = 1200;
        public double wdWidth
        {
            get { return wdWidth_; }
            set { wdWidth_ = value; OnPropertyChanged(); }
        }
        public double wdHeight
        {
            get { return wdHeight_; }
            set { wdHeight_ = value; OnPropertyChanged(); }
        }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        private int _isPanelVisible;
        public int IsPanelVisible
        {
            get { return _isPanelVisible; }
            set { _isPanelVisible = value; OnPropertyChanged(); }
        }
        private double _opacityChange;
        public double OpacityChange
        {
            get { return _opacityChange; }
            set { _opacityChange = value; OnPropertyChanged(); }
        }
        private double _opacityChange1;
        public double OpacityChange1
        {
            get { return _opacityChange1; }
            set { _opacityChange1 = value; OnPropertyChanged(); }
        }
        public ICommand HomeCommand { get; set; }
        public ICommand EmployeeCommand { get; set; }
        public ICommand ProductCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand ReportCommand { get; set; }


        public ICommand VoucherCommand { get; set; }
        public ICommand SupplierCommand { get; set; }
        public ICommand InputInfoCommand { get; set; }
        public ICommand ShowPanelCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand SettingCommand { get; set; }

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
        public AdminMainViewModel()
        {

            Ten = CurrentAccount.Name;
            Anh = DatabaseHelper.LoadAvatar(CurrentAccount.idAccount);
            Email = CurrentAccount.Email;

            IsPanelVisible = 0;  //moi vo la no ko mo menu =hidden
            OpacityChange = 1;
            OpacityChange1 = 0.9;

            HomeCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new HomeView();
            });
            InputInfoCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new InputInfoView();

            });
            EmployeeCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new EmployeeView();

            });
            ProductCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ProductView();

            });
            ProfileCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ProfileView();
            });
            SupplierCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new SupplierView();

            });
            ReportCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new TroubleView();

            });

            VoucherCommand = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new VoucherView();

            });

            ShowPanelCommand = new RelayCommand<AdminMainWindow>(parameter => true, parameter => Show(parameter));
            HidePanelCommand = new RelayCommand<AdminMainWindow>(parameter => true, parameter => Hide(parameter));
            LoadedCommand = new RelayCommand<AdminMainWindow>(parameter => true, parameter => Load(parameter));
            CloseCommand = new RelayCommand<Window>(parameter => true, parameter => { parameter.Close(); });
            SettingCommand = new RelayCommand<AdminMainWindow>(parameter => true, parameter => Setting(parameter));
        }

        public void Show(AdminMainWindow parameter)
        {
            IsPanelVisible = 1;
            OpacityChange = 0.6;
            OpacityChange1 = 0.2;
        }
        public void Hide(AdminMainWindow parameter)
        {
            IsPanelVisible = 2; //Hidden
            OpacityChange = 1;
            OpacityChange1 = 0.9;
        }


        public void Load(AdminMainWindow parameter)
        {
            parameter.DataContext = new AdminMainViewModel();
        }
        public void Setting(AdminMainWindow parameter)
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

