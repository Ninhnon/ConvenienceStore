using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Views.Staff.TroubleWindow;
using Emgu.CV.ML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable enable
namespace ConvenienceStore.ViewModel.TroubleWindowVM
{
    public partial class TroublePageViewModel : BaseViewModel
    {

        private ObservableCollection<Report>? _ListError;
        public ObservableCollection<Report>? ListError
        {
            get => _ListError;
            set { _ListError = value; OnPropertyChanged(); }
        }

        private Report? _SelectedItem;
        public Report? SelectedItem
        {
            get => _SelectedItem;
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private ComboBoxItem? _ItemViewMode;
        public ComboBoxItem? ItemViewMode
        {
            get => _ItemViewMode;
            set { _ItemViewMode = value; OnPropertyChanged(); }
        }

        private string? _Title;
        public string? Title
        {
            get => _Title;
            set { _Title = value; OnPropertyChanged(); }
        }

        private string? _Description;
        public string? Description
        {
            get => _Description;
            set { _Description = value; OnPropertyChanged(); }
        }
        private string? _Status;
        public string? Status
        {
            get => _Status;
            set { _Status = value; OnPropertyChanged(); }
        }
        private ImageSource? _ImageSource;
        public ImageSource? ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(); }
        }

        private ComboBoxItem? _Level;
        public ComboBoxItem? Level
        {
            get => _Level;
            set { _Level = value; OnPropertyChanged(); }
        }

        private Byte[]? _Image;
        public Byte[]? Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }

        public ICommand CancelCM { get; set; }
        //public ICommand FirstLoadCM { get; set; }
        public ICommand FilterListErrorCommand { get; set; }
        public ICommand LoadDetailWindowCM { get; set; }
        public ICommand OpenAddErrorCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand SaveNewTroubleCommand { get; set; }
        public ICommand UpdateReportButtonCommand { get; set; }
        string? filepath;
        bool IsImageChanged = false;
        public static Grid MaskName { get; set; }

        public List<Report> danhsach = new();

        private string? _ReportName;
        public string? ReportName
        {
            get => _ReportName;
            set { _ReportName = value; OnPropertyChanged(); }
        }
        public string RepairCost { get; set; }
        public Report tmpReport { get; set; }

        public TroublePageViewModel()
        {
            
            danhsach = DatabaseHelper.FetchingReportData();

            ListError = new ObservableCollection<Report>(danhsach);
            GetCurrentDate = DateTime.Today;
            //FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            //{
            //    IsLoading = true;
            //    await LoadListError();

            //    //FetchData();
            //    //ListError = new ObservableCollection<Report>(danhsach);
            //    IsLoading = false;
            //});
            CancelCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
                {
                    p.Close();
                });
            FilterListErrorCommand = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, (p) =>
            {
                FilterListError();
            });
            LoadDetailWindowCM = new RelayCommand<System.Windows.Controls.ListView>((p) => { return true; }, (p) =>
            {
                MaskName.Visibility = Visibility.Visible;
                ViewError w = new();
                ReportName = DatabaseHelper.GetName(SelectedItem.StaffId);
                
                if (SelectedItem.RepairCost == null)
                {
                    //w._Finishday.IsEnabled = false;
                    w._cost.IsEnabled = false;
                    //w._Finishday.Visibility = Visibility.Collapsed;
                    //w._startday.Visibility = Visibility.Collapsed;
                    w._cost.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //w._Finishday.IsEnabled = true;
                    w._cost.IsEnabled = true;
                    //w._Finishday.Visibility = Visibility.Visible;
                    //w._startday.Visibility = Visibility.Visible;
                    w._cost.Visibility = Visibility.Visible;
                }
                if (SelectedItem.FinishDate == null)
                {
                    w._Finishday.IsEnabled = false;
                    w._Finishday.Visibility = Visibility.Collapsed;
                }
                else
                {
                    w._Finishday.IsEnabled = true;
                    w._Finishday.Visibility = Visibility.Visible;
                }
                if (SelectedItem.StartDate == null)
                {
                    w._startday.IsEnabled = false;
                    w._startday.Visibility = Visibility.Collapsed;
                }
                else
                {
                    w._startday.IsEnabled = true;
                    w._startday.Visibility = Visibility.Visible;
                }
                w.ShowDialog();
                return;
            });
            OpenAddErrorCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                AddTrouble w1 = new();
                MaskName.Visibility = Visibility.Visible;
                w1.StaffName.Text = CurrentAccount.Name.ToString();
                w1.cbxStatus.Text = "Chờ tiếp nhận";
                w1.ShowDialog();
            });
            SaveErrorCM = new RelayCommand<AddError>((p) => { if (IsSaving) return false; return true; }, (p) =>
            {
                IsSaving = true;
                SaveErrorFunc(p);
                IsSaving = false;
            });
            UploadImageCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openfile = new()
                {
                    Title = "Select an image",
                    Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png"
                };
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    filepath = openfile.FileName;
                    ImageSource = new BitmapImage(new Uri(filepath));
                    Image = File.ReadAllBytes(filepath);
                    IsImageChanged = true;
                }
                IsImageChanged = false;

            });
            UploadImageCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
            OpenFileDialog openfile = new()
            {
                Title = "Select an image",
                Filter = "Image File (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg; *.png"
            };
                if (openfile.ShowDialog() == DialogResult.OK)
                {
                    filepath = openfile.FileName;
                    ImageSource = new BitmapImage(new Uri(filepath));
                }
            });
            SaveNewTroubleCommand = new RelayCommand<AddTrouble>((p) => { return true; }, (p) =>
            {
                IsSaving = true;
                Save(p);
                MaskName.Visibility = Visibility.Collapsed;
                IsSaving = false;
            });
            UpdateReportButtonCommand = new RelayCommand<EditTrouble>((p) => { return true; }, (p) =>
            {
                IsSaving = true;
                tmpReport = SelectedItem;
                Update(p);
                MaskName.Visibility = Visibility.Collapsed;
                IsSaving = false;
            });
            LoadEditErrorCM = new RelayCommand<EditTrouble>((p) => { return true; }, (p) =>
            {
                EditTrouble w1 = new();
                LoadEditError(w1);
                MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });
            UpdateErrorCM = new RelayCommand<EditError>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                UpdateErrorFunc(p);
                isSaving = false;
            });

            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
            CloseCM = new RelayCommand<Window>((p) => { if (IsSaving) return false; return true; }, (p) =>
             {
                 MaskName.Visibility = Visibility.Collapsed;
                 p.Close();
             });
            MouseMoveCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
            {
                Window w = Window.GetWindow(p);
                w?.DragMove();
            });
        }

        public void FilterListError()
        {
            if (ListError != null)
            {
                ListError.Clear();
                if (ItemViewMode.Content.ToString() == "Tất cả")
                {
                    for (int i = 0; i < danhsach.Count; ++i)
                    {
                        ListError.Add(danhsach[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < danhsach.Count; ++i)
                    {
                        if (danhsach[i].Status == ItemViewMode.Content.ToString())
                        {
                            ListError.Add(danhsach[i]);
                        }
                    }
                }
            }
        }
        //public async Task GetData()
        //{
        //    //GetAllError = new ObservableCollection<Report>(await Task.Run(() => TroubleService.Ins.GetAllTrouble()));
        //    ListError = new ObservableCollection<Report>(GetAllError);
        //}
        void RenewWindowData()
        {
            Title = null;
            Description = null;
            ImageSource = null;
            filepath = null;
        }

        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(Title)
                     && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Level.Content.ToString());
        }
    }
}
