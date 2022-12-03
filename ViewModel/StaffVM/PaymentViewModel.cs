using ConvenienceStore.Model;
using ConvenienceStore.ViewModel.Lam.Helpers;
using ConvenienceStore.ViewModel.MainBase;
using ConvenienceStore.Views.Staff;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class PaymentViewModel : BaseViewModel
    {
        #region ICommand Payment
        public ICommand AddToCart { get; set; }
        public ICommand IncreaseProductAmount { get; set; }
        public ICommand DecreaseProductAmount { get; set; }
        public ICommand RemoveProduct { get; set; }
        public ICommand SearchProductName { get; set; }
        public ICommand FilterType { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand OpenReceiptPage { get; set; }
        public ICommand ScrollToEndListBox { get; set; }
        #endregion

        #region Icommand Receipt
        public ICommand LoadReceiptPage { get; set; }
        public ICommand CancelReceiptCM { get; set; }
        public ICommand PrintBillCM { get; set; }
        #endregion

        // PaymentView
        private ObservableCollection<Consignment> _List;
        public ObservableCollection<Consignment> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Consignment> _FilteredList;
        public ObservableCollection<Consignment> FilteredList { get { return _FilteredList; } set { _FilteredList = value; OnPropertyChanged(); } }

        private ObservableCollection<BillDetail> _ShoppingCart;
        public ObservableCollection<BillDetail> ShoppingCart { get { return _ShoppingCart; } set { _ShoppingCart = value; OnPropertyChanged(); } }

        private ObservableCollection<Bill> _ReceiptInfo;
        public ObservableCollection<Bill> ReceiptInfo { get { return _ReceiptInfo; } set { _ReceiptInfo = value; OnPropertyChanged(); } }

        private Consignment _SelectedItem;
        public Consignment SelectedItem { get { return _SelectedItem; } set { _SelectedItem = value; OnPropertyChanged(); } }

        private BillDetail _SelectedBillDetail;
        public BillDetail SelectedBillDetail { get { return _SelectedBillDetail; } set { _SelectedBillDetail = value; OnPropertyChanged(); } }

        private string _SearchContent;
        public string SearchContent { get { return _SearchContent; } set { _SearchContent = value; OnPropertyChanged(); } }

        private ComboBoxItem _ComboBoxCategory;
        public ComboBoxItem ComboBoxCategory { get { return _ComboBoxCategory; } set { _ComboBoxCategory = value; OnPropertyChanged(); } }

        public static Grid MaskName { get; set; }

        // ReceiptView
        private ObservableCollection<double> _ItemTotalPrice;
        public ObservableCollection<double> ItemTotalPrice { get { return _ItemTotalPrice; } set { _ItemTotalPrice = value; OnPropertyChanged(); } }

        private int _TotalBill;
        public int TotalBill { get { return _TotalBill; } set { _TotalBill = value; OnPropertyChanged(); } }


        public List<Consignment> danhsach = new();
        public PaymentViewModel()
        {
            // Load tất cả sản phẩm trong danh sách lên List
            //List = new ObservableCollection<Product>(DataProvider.Ins.DB.Products);
            //Product a = DataProvider.Ins.DB.Products.Where(x => x.Barcode.Equals("8934563653132")).FirstOrDefault();
            //Product z = List.FirstOrDefault();
            //Product a = new Product();
            //a.Consignments.OrderBy(x => x.ExpiryDate).Reverse().FirstOrDefault();
            //List = new ObservableCollection<Product>(DataProvider.Ins.DB.Products);
            //foreach(Product product in List)
            //{
            //    product.
            //}
            //List<Consignment> b = a.Consignments.OrderBy(x => x.ManufacturingDate).Reverse().ToList();
            //int? z = DataProvider.Ins.DB.BillDetails.Where(x => x.ProductId.Equals("8934563653132")).Sum(x => x.Quantity); 


            //List = new ObservableCollection<Consignment>(DataProvider.Ins.DB.Consignments.OrderByDescending(x => x.ExpiryDate).Distinct().ToList());
            danhsach = DatabaseHelper.FetchingConsignmentData();
            List = new ObservableCollection<Consignment>(danhsach);

            FilteredList = List;
            ShoppingCart = new ObservableCollection<BillDetail>();

            // Thêm sản phẩm vào giỏ hàng
            AddToCart = new RelayCommand<BillDetail>((p) => 
            {
                return true;
            }, (p) =>
            {
                //Kiểm tra trong giỏ hàng đã có hay chưa, có rồi thì không thêm vào
                var checkExistItem = ShoppingCart.Where(x => x.ProductId == SelectedItem.Product.Barcode);
                if (checkExistItem.Count() != 0 || checkExistItem == null)
                    return;
                else
                {
                    //SelectedItem.Quantity = 1;
                    BillDetail billDetail = new BillDetail();
                    billDetail.Product = SelectedItem.Product;
                    billDetail.Quantity = 1;
                    billDetail.ProductId = SelectedItem.ProductId;
                    billDetail.TotalPrice = SelectedItem.OutputPrice;

                    SelectedBillDetail = billDetail;
                    ShoppingCart.Add(billDetail);
                }
            }
            );

            //Tăng giảm số lượng, xóa khỏi giỏ hàng
            IncreaseProductAmount = new RelayCommand<BillDetail>((p) =>
            {
                return true;
            }, (p) =>
            {
                BillDetail item = SelectedBillDetail;
                if (item != null)
                {
                    item.TotalPrice = item.TotalPrice / item.Quantity * (item.Quantity + 1);
                    item.Quantity++;
                }
            }
            );
            DecreaseProductAmount = new RelayCommand<BillDetail>((p) =>
            {
                return true;
            }, (p) =>
            {
                BillDetail item = SelectedBillDetail;
                if (item != null)
                {
                    if (item.Quantity <= 1)
                        item.Quantity = 1;
                    else
                    {
                        item.TotalPrice = item.TotalPrice / item.Quantity * (item.Quantity - 1);
                        item.Quantity--;
                    }
                }
            }
            );
            RemoveProduct = new RelayCommand<BillDetail>((p) =>
            {
                return true;
            }, (p) =>
            {
                BillDetail item = SelectedBillDetail;
                if (item != null)
                {
                    ShoppingCart.Remove(item);
                }
            }
            );

            //Tìm kiếm & lọc
            SearchProductName = new RelayCommand<TextBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                var tbx = p;

                FilteredList = List;
                if (tbx.Text != "")
                {
                    FilteredList = new ObservableCollection<Consignment>(FilteredList.Where(x => x.Product.Title.ToLower().Contains(tbx.Text.ToLower())).ToList());
                }
            }
            );
            FilterType = new RelayCommand<object>((p) => 
            {
                return true;
            }, (p) =>
            {
                //Lọc sản phẩm theo loại
                //if (ComboBoxCategory.Content.ToString() == "Tất cả")
                //{
                //    FilteredList = new ObservableCollection<Consignment>(DataProvider.Ins.DB.Consignments.OrderByDescending(x => x.ExpiryDate).Distinct().ToList());
                //}
                //else
                //{
                //    FilteredList = new ObservableCollection<Consignment>(DataProvider.Ins.DB.Consignments.Where(x => x.Product.Type == ComboBoxCategory.Content.ToString()).OrderByDescending(x => x.ExpiryDate).Distinct().ToList());
                //}
                //Lưu lại danh sách các sản phẩm, hỗ trợ việc tìm kiếm của SearchProductName
                List = FilteredList;
            });

            //Mask
            MaskNameCM = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                MaskName = p;
            });

            OpenReceiptPage = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                Receipt wd = new Receipt();

                MaskName.Visibility = Visibility.Visible;

                TotalBill = 0;
                foreach (BillDetail bd in ShoppingCart)
                {
                    TotalBill += (int)bd.TotalPrice;
                }

                wd.ShowDialog();
            });

            CancelReceiptCM = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
                MaskName.Visibility = Visibility.Hidden;
            });

            ScrollToEndListBox = new RelayCommand<ListBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.ScrollIntoView(SelectedBillDetail);

            });
        }

    }
}
