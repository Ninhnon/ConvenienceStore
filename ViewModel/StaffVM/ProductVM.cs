using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Views.Staff.ProductWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class ProductVM : BaseViewModel
    {

        private ObservableCollection<Products>? _List;
        public ObservableCollection<Products>? List { get => _List; set { _List = value; OnPropertyChanged(); } }
        public List<Products> danhsach = new();

        private string? _Text;
        public string? Text
        {
            get => _Text;
            set { _Text = value; OnPropertyChanged(); }
        }
        private ComboBoxItem? _ItemViewMode;
        public ComboBoxItem? ItemViewMode
        {
            get => _ItemViewMode;
            set { _ItemViewMode = value; OnPropertyChanged(); }
        }
        private ComboBoxItem? _StatusName;
        public ComboBoxItem? StatusName
        {
            get => _StatusName;
            set { _StatusName = value; OnPropertyChanged(); }
        }
        private Products _SelectedItem;
        public Products SelectedItem
        {
            get => _SelectedItem;
            set { _SelectedItem = value; OnPropertyChanged(); }
        }
        //public ICommand AddCommand { get; set; }
        //public ICommand LoadInforProductCM { get; set; }
        //public ICommand SelectedCM { get; set; }
        public ICommand FilterListProductCommand { get; set; }
        //public ICommand FilterStatusProductCommand { get; set; }
        public ICommand LoadDetailWindowCM { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand OpenQRCommand { get; set; }
        public ICommand RefreshWindowCM { get; set; }
        public Grid MaskName { get; set; }
        public ProductVM()
        {
            danhsach= DatabaseHelper.FetchingProductData();
            List = new ObservableCollection<Products>(danhsach);
            FilterListProductCommand = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                FilterListProducts();
            });
            //FilterStatusProductCommand = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, (p) =>
            //{
            //    FilterStatusProducts();
            //});
            LoadDetailWindowCM = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                //MaskName.Visibility = Visibility.Visible;
                ViewProduct w = new();
                w.ShowDialog();
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
            OpenQRCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                QRView w1 = new();
                w1.ShowDialog();
                //MaskName.Visibility = Visibility.Visible;
                //while (Text == null) ;
                if (Text != null)
                {
                    List<Products> Search = new();
                    foreach (Products s in danhsach)
                    {
                        if (s.BarCode == Text) Search.Add(s);
                    }
                    List = new ObservableCollection<Products>(Search);
                }
            });
            RefreshWindowCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                danhsach = DatabaseHelper.FetchingProductData();
                List = new ObservableCollection<Products>(danhsach);
            });
        }
        //private void SearchQR()
        //{
        //    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(List);
        //    view.Filter = Filter;
        //    CollectionViewSource.GetDefaultView(List).Refresh();
        //}
        //private bool Filter(object item)
        //{
        //    if (String.IsNullOrEmpty(Text))
        //        return true;
        //    else
        //        return ((Products)item).BarCode.Contains(Text, StringComparison.OrdinalIgnoreCase);
        //}
        public void FilterListProducts()
        {
            List.Clear();
            if (ItemViewMode.Content.ToString() == "Tất cả")
            {
                for (int i = 0; i < danhsach.Count; ++i)
                {
                    List.Add(danhsach[i]);
                }
            }
            else
            {
                for (int i = 0; i < danhsach.Count; ++i)
                {
                    if (danhsach[i].Type == ItemViewMode.Content.ToString())
                    {
                        List.Add(danhsach[i]);
                    }
                }
            }
        }
        //public void FilterStatusProducts()
        //{
        //    List.Clear();
        //    DateTime t = DateTime.Now;
        //    TimeSpan h = new(10, 0, 0, 0);
        //    if (StatusName.Content.ToString() == "Tất cả")
        //    {
        //        for (int i = 0; i < danhsach.Count; ++i)
        //        {
        //            List.Add(danhsach[i]);
        //        }
        //    }
        //    else if (StatusName.Content.ToString() == "Hết hạn sử dụng")
        //    {
        //        for (int i = 0; i < danhsach.Count; ++i)
        //        {
        //            if (t > danhsach[i].ExpiryDate)
        //            {
        //                List.Add(danhsach[i]);
        //            }
        //        }
        //    }
        //    else if (StatusName.Content.ToString() == "Khác")
        //    {
        //        {
        //            for (int i = 0; i < danhsach.Count; ++i)
        //            {
        //                if (t <= danhsach[i].ExpiryDate)
        //                {
        //                    List.Add(danhsach[i]);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < danhsach.Count; ++i)
        //        {
        //            if ((t - (danhsach[i].ExpiryDate) <= h) && (t > danhsach[i].ExpiryDate))
        //            {
        //                List.Add(danhsach[i]);
        //            }
        //        }
        //    }
        //}

    }
}
