using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Staff;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace ConvenienceStore.ViewModel.StaffVM
{
    public class HistoryViewModel : BaseViewModel
    {
        #region Region for History Page ICommand
        public ICommand SearchBillCM { get; set; }
        public ICommand GetMoreBillDetailCM { get; set; }
        public ICommand SelectionChangedCM { get; set; }
        public ICommand LoadHistoryPageCM { get; set; }
        public ICommand MaskNameCM { get; set; }
        #endregion

        #region Region for History Receipt ICommand
        public ICommand CancelReceiptCM { get; set; }
        #endregion

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
            bills = DatabaseHelper.FetchingBillsData();
            BillList = new ObservableCollection<Bills>(bills);
            (sender as BackgroundWorker).ReportProgress(0);
        }

        public List<Bills> bills = new List<Bills>();
        public List<BillDetails> billDetails = new List<BillDetails>();

        private ObservableCollection<Bills> _BillList;
        public ObservableCollection<Bills> BillList { get { return _BillList; } set { _BillList = value; OnPropertyChanged(); } }

        private Bills _SelectedItem;
        public Bills SelectedItem { get { return _SelectedItem; } set { _SelectedItem = value; OnPropertyChanged(); } }

        private ComboBoxItem _ComboBoxCategory;
        public ComboBoxItem ComboBoxCategory { get { return _ComboBoxCategory; } set { _ComboBoxCategory = value; OnPropertyChanged(); } }

        private string _SearchContent;
        public string SearchContent { get { return _SearchContent; } set { _SearchContent = value; OnPropertyChanged(); } }

        private Bills _BillInfo;
        public Bills BillInfo { get { return _BillInfo; } set { _BillInfo = value; OnPropertyChanged(); } }

        private HistoryReceipt _ReceiptPage;
        public HistoryReceipt ReceiptPage { get { return _ReceiptPage; } set { _ReceiptPage = value; OnPropertyChanged(); } }

        private ObservableCollection<BillDetails> _ShoppingCart;
        public ObservableCollection<BillDetails> ShoppingCart { get { return _ShoppingCart; } set { _ShoppingCart = value; OnPropertyChanged(); } }

        public static Grid MaskName { get; set; }

        public HistoryViewModel()
        {
            worker = new BackgroundWorker { WorkerReportsProgress = true };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;

            GetMoreBillDetailCM = new RelayCommand<DataGrid>((p) =>
            {
                return true;
            }, (p) =>
            {
                BillInfo = SelectedItem;
                billDetails = DatabaseHelper.FetchingBillDetailsData(BillInfo);
                ShoppingCart = new ObservableCollection<BillDetails>(billDetails);
                MaskName.Visibility = Visibility.Visible;
                ReceiptPage = new HistoryReceipt();
                ReceiptPage.ShowDialog();
            });

            CancelReceiptCM = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ReceiptPage.Close();
                MaskName.Visibility = Visibility.Collapsed;
            });

            LoadHistoryPageCM = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                BillList = null;
                LoadData();
                //bills = DatabaseHelper.FetchingBillsData();
                //BillList = new ObservableCollection<Bills>(bills);
            });

            SearchBillCM = new RelayCommand<TextBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (SearchContent == "" || SearchContent == null)
                    BillList = new ObservableCollection<Bills>(bills);
                else if (ComboBoxCategory.Content.ToString() == "Số hóa đơn")
                    BillList = new ObservableCollection<Bills>((bills).Where(x => x.BillId.ToString() == SearchContent).ToList());
                else if (ComboBoxCategory.Content.ToString() == "Tên khách hàng")
                    BillList = new ObservableCollection<Bills>((bills).Where(x => x.CustomerName.ToLower().Contains(SearchContent.ToLower())).ToList());
                else
                    BillList = new ObservableCollection<Bills>((bills).Where(x => x.UserName.ToLower().Contains(SearchContent.ToLower())).ToList());
            });

            MaskNameCM = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                MaskName = p;
            });
        }
    }
}
