using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.ViewModel.StaffVM.TroubleWindowVM;
using ConvenienceStore.Views.Staff.TroubleWindow;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
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
        private BackgroundWorker worker;
        private bool _IsLoading;
        public bool IsLoading { get { return _IsLoading; } set { _IsLoading = value; OnPropertyChanged(); } }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            IsLoading = false;
        }

        public void LoadData()
        {
            IsLoading = true;
            try
            {
                worker.RunWorkerAsync();
            }
            catch
            {
                //get some more time for worker
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {

            Thread.Sleep(1000);
            danhsach = DatabaseHelper.FetchingReportData();
            ListError = new ObservableCollection<Report>(danhsach);
            (sender as BackgroundWorker).ReportProgress(0);
        }

        private ObservableCollection<Report>? _ListError;
        public ObservableCollection<Report>? ListError
        {
            get => _ListError;
            set { _ListError = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Report>? _FilteredList;
        public ObservableCollection<Report>? FilteredList
        {
            get => _FilteredList;
            set { _FilteredList = value; OnPropertyChanged(); }
        }


        private Report _SelectedItem;
        public Report SelectedItem
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
        public ICommand UploadImageCommand { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand SaveNewTroubleCommand { get; set; }
        public ICommand UpdateReportButtonCommand { get; set; }
        public ICommand LoadTroublePageCM { get; set; }
        public Grid MaskName { get; set; }

        public List<Report> danhsach = new();

        private string? _ReportName;
        public string? ReportName
        {
            get => _ReportName;
            set { _ReportName = value; OnPropertyChanged(); }
        }
        public string RepairCost { get; set; }
        public Report tmpReport { get; set; }
        public int Id { get; set; }
        public DateTime Se { get; set; }

        public Snackbar TroubleSnackbar;
        private ComboBoxItem _ComboBoxCategory;
        public ComboBoxItem ComboBoxCategory { get { return _ComboBoxCategory; } set { _ComboBoxCategory = value; OnPropertyChanged(); } }

        public BindingTroubleSnackbar BindingTroubleSnackbar { get; set; }

        public TroublePageViewModel()
        {
            worker = new BackgroundWorker { WorkerReportsProgress = true };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;

            BindingTroubleSnackbar = new BindingTroubleSnackbar(this);

            //danhsach = DatabaseHelper.FetchingReportData();
            //ListError = new ObservableCollection<Report>(danhsach);
            //MaskName.Visibility = Visibility.Collapsed;

            GetCurrentDate = DateTime.Today;
            //FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            //{
            //    IsLoading = true;
            //    await LoadListError();

            //    //FetchData();
            //    //ListError = new ObservableCollection<Report>(danhsach);
            //    IsLoading = false;
            //});
            LoadTroublePageCM = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ListError = null;
                LoadData();
            });

            MaskNameCM = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                MaskName = p;
            });
            CancelCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
                {
                    MaskName.Visibility = Visibility.Collapsed;
                    p.Close();
                });
            FilterListErrorCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (ComboBoxCategory.Content.ToString() == "Tất cả")
                    FilteredList = new ObservableCollection<Report>(danhsach);
                else
                    FilteredList = new ObservableCollection<Report>((danhsach).Where(x => x.Status == ComboBoxCategory.Content.ToString()).ToList());
                //Lưu lại danh sách các sản phẩm, hỗ trợ việc tìm kiếm của SearchProductName
                ListError = FilteredList;
            });
            LoadDetailWindowCM = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                ViewError w = new();
                ReportName = DatabaseHelper.GetName(SelectedItem.StaffId);

                if (SelectedItem.RepairCost == null)
                {
                    w._cost.IsEnabled = false;
                    w._cost.Visibility = Visibility.Collapsed;
                }
                else
                {
                    w._cost.IsEnabled = true;
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
                MaskName.Visibility = Visibility.Visible;
                AddTrouble w1 = new();
                w1.StaffName.Text = CurrentAccount.Name.ToString();
                w1.cbxStatus.Text = "Chờ tiếp nhận";
                w1.ShowDialog();
            });
            UploadImageCommand = new RelayCommand<ImageBrush>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "Image files|*.jpeg;*.jpg;*.png";
                openDialog.FilterIndex = -1;

                BitmapImage bi = new BitmapImage();

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    var bytes = File.ReadAllBytes(openDialog.FileName);
                    string s = Convert.ToBase64String(bytes);

                    bi.BeginInit();
                    bi.StreamSource = new MemoryStream(Convert.FromBase64String(s));
                    bi.EndInit();
                }
                try
                {
                    p.ImageSource = bi;
                }
                catch
                { }
            });
            SaveNewTroubleCommand = new RelayCommand<AddTrouble>((p) => { return true; }, (p) =>
            {
                IsSaving = true;
                Save(p, TroubleSnackbar);
                MaskName.Visibility = Visibility.Collapsed;
                IsSaving = false;

            });
            UpdateReportButtonCommand = new RelayCommand<EditTrouble>((p) => { return true; }, (p) =>
            {
                IsSaving = true;
                Update(p, tmpReport, TroubleSnackbar);
                IsSaving = false;
            });
            LoadEditErrorCM = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                EditTrouble w1 = new();
                Id = SelectedItem.Id;
                Se = SelectedItem.SubmittedAt;
                LoadEditError(w1);
                w1.ShowDialog();
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
                if (ItemViewMode.Content.ToString() == "Toàn bộ")
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
        void RenewWindowData()
        {
            Title = null;
            Description = null;
            ImageSource = null;
        }

        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(Title)
                     && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Level.Content.ToString());
        }
    }
}
