using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public ICommand SearchCustomerIdCM { get; set; }
        public ICommand CompleteReceiptCM { get; set; }
        #endregion

        // PaymentView
        private ObservableCollection<Products> _List;
        public ObservableCollection<Products> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Products> _FilteredList;
        public ObservableCollection<Products> FilteredList { get { return _FilteredList; } set { _FilteredList = value; OnPropertyChanged(); } }

        private ObservableCollection<BillDetails> _ShoppingCart;
        public ObservableCollection<BillDetails> ShoppingCart { get { return _ShoppingCart; } set { _ShoppingCart = value; OnPropertyChanged(); } }

        private Products _SelectedItem;
        public Products SelectedItem { get { return _SelectedItem; } set { _SelectedItem = value; OnPropertyChanged(); } }

        private BillDetails _SelectedBillDetail;
        public BillDetails SelectedBillDetail { get { return _SelectedBillDetail; } set { _SelectedBillDetail = value; OnPropertyChanged(); } }

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

        private int? _CustomerId;
        public int? CustomerId { get { return _CustomerId; } set { _CustomerId = value; OnPropertyChanged(); } }

        //public Receipt ReceiptPage

        public List<Products> products = new List<Products>();

        public List<Customer> customers = new List<Customer>();

        public Model.Staff.Bill bill = new Model.Staff.Bill();


        public PaymentViewModel()
        {
            products = DatabaseHelper.FetchingProductData();
            List = new ObservableCollection<Products>(products);

            FilteredList = List;
            ShoppingCart = new ObservableCollection<BillDetails>();

            // Thêm sản phẩm vào giỏ hàng
            AddToCart = new RelayCommand<BillDetail>((p) =>
            {
                return true;
            }, (p) =>
            {
                //Kiểm tra trong giỏ hàng đã có hay chưa, có rồi thì không thêm vào
                var checkExistItem = ShoppingCart.Where(x => x.ProductId == SelectedItem.BarCode);
                if (checkExistItem.Count() != 0 || checkExistItem == null)
                    return;
                else
                {
                    //SelectedItem.Quantity = 1;
                    BillDetails billDetail = new BillDetails();
                    billDetail.ProductId = SelectedItem.BarCode;
                    billDetail.Quantity = 1;
                    billDetail.TotalPrice = SelectedItem.Price;
                    billDetail.Title = SelectedItem.Title;
                    billDetail.Image = SelectedItem.Image;

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
                BillDetails item = SelectedBillDetail;
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
                BillDetails item = SelectedBillDetail;
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
                BillDetails item = SelectedBillDetail;
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
                TextBox? tbx = p;

                FilteredList = List;
                if (tbx.Text != "")
                {
                    FilteredList = new ObservableCollection<Products>(FilteredList.Where(x => x.Title.ToLower().Contains(tbx.Text.ToLower())).ToList());
                }
            }
            );
            FilterType = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                //Lọc sản phẩm theo loại
                if (ComboBoxCategory.Content.ToString() == "Tất cả")
                    FilteredList = new ObservableCollection<Products>(products);
                else
                    FilteredList = new ObservableCollection<Products>((products).Where(x => x.Type == ComboBoxCategory.Content.ToString()).ToList());

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
                if (ShoppingCart.Count == 0)
                    return false;
                return true;
            }, (p) =>
            {
                Receipt wd = new Receipt();

                MaskName.Visibility = Visibility.Visible;

                TotalBill = 0;
                foreach (BillDetails bd in ShoppingCart)
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

            SearchCustomerIdCM = new RelayCommand<TextBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                customers = DatabaseHelper.FetchingCustomerData();
                var checkCustomerId = customers.Where(x => Convert.ToString(x.Id) == p.Text);

                if (p.Text == null)
                    CustomerId = null;

                if (checkCustomerId.Count() == 1)
                {
                    CustomerId = Convert.ToInt32(p.Text);
                    MessageBoxCustom mbSuccess = new MessageBoxCustom("Thông báo", "Mã khách hàng hợp lệ", MessageType.Success, MessageButtons.OK);
                    mbSuccess.ShowDialog();
                }    
                else
                {
                    p.Text = null;
                    CustomerId = null;
                    MessageBoxCustom mbFailed = new MessageBoxCustom("Cảnh báo", "Mã khách hàng không hợp lệ", MessageType.Error, MessageButtons.OK);
                    mbFailed.ShowDialog();
                }
            });

            CompleteReceiptCM = new RelayCommand<Button>((p) =>
            {
                return true;
            }, (p) =>
            {
                DatabaseHelper.InsertBill(CustomerId, TotalBill);
                bill = DatabaseHelper.FetchingBillData().LastOrDefault();
                if (bill == null)
                    return;

                foreach (BillDetails bd in ShoppingCart)
                {
                    bd.BillId = bill.Id;
                }

                p.IsEnabled = false;
            });
        }
    }
}
