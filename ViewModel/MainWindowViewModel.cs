using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ConvenienceStore.Model;
using ConvenienceStore.Resources.UserControlCustom;
using ConvenienceStore.ViewModel.Lam;
using ConvenienceStore.Views;

namespace ConvenienceStore.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
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
        public ICommand ShowPanelCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand SizeChangedCommand { get; set; }

        public MainWindowViewModel()
        {
            IsPanelVisible = 0;  //moi vo la no ko mo menu =hidden
            OpacityChange = 1;
            OpacityChange1 = 0.9;
            CurrentView = new InputInfoVM();
            ShowPanelCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Show(parameter));
            HidePanelCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Hide(parameter));
            HomeCommand = new RelayCommand<HomeView>(parameter => true, parameter => Home(parameter));
            EmployeeCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => Employee(parameter));
            ProductCommand = new RelayCommand<InputInfoView>(parameter => true, parameter => Product(parameter));
            ProfileCommand = new RelayCommand<ProfileView>(parameter => true, parameter => Profile(parameter));
            ReportCommand = new RelayCommand<ReportView>(parameter => true, parameter => ReportOpen(parameter));
            SizeChangedCommand = new RelayCommand<MainWindow>(parameter => true, parameter => SizeChanged(parameter));
        }

        public void Show(MainWindow parameter)
        {

            IsPanelVisible = 1;
            OpacityChange = 0.6;
            OpacityChange1 = 0.2;
        }
        public void Hide(MainWindow parameter)
        {
            IsPanelVisible = 2; //Hidden
            OpacityChange = 1;
            OpacityChange1 = 0.9;
        }

        public void Home(HomeView mainWindow)
        {
            CurrentView = new HomeViewModel();
        }
        public void Employee(EmployeeView parameter)
        {
            CurrentView = new EmployeeViewModel();
        }
        public void Product(InputInfoView parameter)
        {
            CurrentView = new InputInfoVM();
        }
        public void Profile(ProfileView parameter)
        {
            CurrentView = new ProfileViewModel();
        }
        public void ReportOpen(ReportView parameter)
        {
            CurrentView = new ReportViewModel();
        }

        public void SizeChanged(MainWindow parameter)
        {
            wdHeight = parameter.ActualHeight;
            wdWidth = parameter.ActualWidth;



        }

    }
}

