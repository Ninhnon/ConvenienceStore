using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.StaffVM
{
    internal class VoucherVM : BaseViewModel
    {
        public static Grid ShadowMask { get; set; }
        //public Frame mainFrame { get; set; }
        //public Card ButtonView { get; set; }
        //public ICommand SavemainFrameNameCM { get; set; }
        //public ICommand StoreButtonNameCM { get; set; }
        //public ICommand LoadViewCM { get; set; }
        //public ICommand LoadEdit_InforViewCM { get; set; }
        //public ICommand LoadDeleteVoucherCM { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        //public ICommand RefreshEmailListCM { get; set; }
        //public ICommand LoadInforBigVoucherCM { get; set; }
        public ICommand SelectedCM { get; set; }
        //public string ReleaseId
        //{
        //    get { return _ReleaseId; }
        //    set { _ReleaseId = value; OnPropertyChanged(); }
        //}
        //string _ReleaseName;
        //public string ReleaseName
        //{
        //    get { return _ReleaseName; }
        //    set { _ReleaseName = value; OnPropertyChanged(); }
        //}
        //private DateTime startDate;
        //public DateTime StartDate
        //{
        //    get { return startDate; }
        //    set { startDate = value; OnPropertyChanged(); }
        //}
        private DateTime getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return getCurrentDate; }
            set { getCurrentDate = value; OnPropertyChanged(); }
        }
        //private DateTime finishDate;
        //public DateTime FinishDate
        //{
        //    get { return finishDate; }
        //    set { finishDate = value; OnPropertyChanged(); }
        //}
        //private int _ParValue;
        //public int ParValue
        //{
        //    get { return _ParValue; }
        //    set { _ParValue = value; OnPropertyChanged(); }
        //}

        //private static bool _Status;
        //public static bool Status
        //{
        //    get { return _Status; }
        //    set { _Status = value; }
        //}

        private string? _StatusName;
        public string? StatusName
        {
            get { return _StatusName; }
            set { _StatusName = value; OnPropertyChanged(); }
        }
        //string _Type;
        //public string Type
        //{
        //    get { return _Type; }
        //    set { _Type = value; OnPropertyChanged(); }
        //}
        private Vouchers? _SelectedItem;
        public Vouchers? SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
            }
        }

        //private string _SelectedVC;
        //public string SelectedVC
        //{
        //    get => _SelectedVC;
        //    set
        //    {
        //        _SelectedVC = value;
        //        OnPropertyChanged();
        //    }
        //}

        private ObservableCollection<Vouchers>? _List;
        public ObservableCollection<Vouchers>? List { get => _List; set { _List = value; OnPropertyChanged(); } }
        public List<Vouchers> danhsach = new();


        public VoucherVM()
        {
            danhsach = DatabaseHelper.FetchingVoucherData();
            List = new ObservableCollection<Vouchers>(danhsach);

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GetCurrentDate = DateTime.Today;
                danhsach = DatabaseHelper.FetchingVoucherData();
                List = new ObservableCollection<Vouchers>(danhsach);
            });
            SelectedCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (StatusName == "Kich hoat") FilterDone();
                else if (StatusName == "Chua kich hoat") FilterNot();
                else FilterAll();
            });

        }
        public void FilterDone()
        {
            ObservableCollection<Vouchers> temp = new();
            foreach (Vouchers item in danhsach)
            {
                if (item.Status == 1)
                    temp.Add(item);
            }
            List = new ObservableCollection<Vouchers>(temp);
        }
        public void FilterNot()
        {
            ObservableCollection<Vouchers> temp = new();
            foreach (Vouchers item in danhsach)
            {
                if (item.Status != 1)
                    temp.Add(item);
            }
            List = new ObservableCollection<Vouchers>(temp);
        }
        public void FilterAll()
        {
            ObservableCollection<Vouchers> temp = new();
            foreach (Vouchers item in danhsach)
                temp.Add(item);
            List = new ObservableCollection<Vouchers>(temp);
        }

        public int GetAllVoucherReleases()
        {
            return danhsach.Count;
        }
    }
}
