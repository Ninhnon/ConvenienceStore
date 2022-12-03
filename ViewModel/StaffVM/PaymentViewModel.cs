using CinemaManagement.Utils;
using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Views.Staff;
using Emgu.CV.Structure;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public List<Products> products = new List<Products>();
        public List<ConvenienceStore.Model.Staff.Bill> bill = new List<ConvenienceStore.Model.Staff.Bill>();
        public SqlConnection connection = new("Data Source=DESKTOP-RTH9F0I;Initial Catalog=ConvenienceStore;Integrated Security=True");

        public byte[] Image;

        string productQuery = @"select Barcode,Title,ProductionSite,Image,InputPrice,OutputPrice,Stock,ManufacturingDate,ExpiryDate,Discount,Type
        from Consignment c,Product p,
        ( 
        select ProductId, min([ExpiryDate]) e
        from Consignment
        where Stock>0
        group by ProductId
        ) h
        where c.ProductId=p.Barcode and h.ProductId=c.ProductId and h.e = c.ExpiryDate
        order by ExpiryDate";

        string billQuery = @"select * from Bill";

        //string insertBill = @"insert into "

        public void FetchProductData()
        {
            SqlCommand cmd = new(productQuery, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Products()
                {
                    BarCode = reader.GetString(0),
                    Title = reader.GetString(1),
                    ProductionSite = reader.GetString(2),
                    Image = (byte[])reader["Image"],
                    Cost = reader.GetInt32(4),
                    Price = reader.GetInt32(5),
                    Stock = reader.GetInt32(6),
                    ManufacturingDate = reader.GetDateTime(7),
                    ExpiryDate = reader.GetDateTime(8),
                    Discount = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                    Type = reader.IsDBNull(10) ? null : reader.GetString(10),
                });
            }
            reader.Close();
        }

        public void FetchBillData()
        {
            SqlCommand cmd = new(billQuery, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bill.Add(new ConvenienceStore.Model.Staff.Bill()
                {
                    Id = reader.GetInt32(0),
                    BillDate = reader.GetDateTime(1),
                    CustomerId = reader.GetInt32(2),
                    UserId = reader.GetInt32(3),
                    Price = reader.GetInt32(3),
                });
            }
        }

        public PaymentViewModel()
        {
            connection.Open();
            FetchProductData();

            List = new ObservableCollection<Products>(products);
            FilteredList = List;
            ShoppingCart = new ObservableCollection<BillDetails>();

            Image = List.FirstOrDefault().Image;
            connection.Close();

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
                {
                    //FilteredList = new ObservableCollection<Products>(DataProvider.Ins.DB.Consignments.OrderByDescending(x => x.ExpiryDate).Distinct().ToList());
                    FilteredList = new ObservableCollection<Products>(products);
                }
                else
                {
                    //FilteredList = new ObservableCollection<Consignment>(DataProvider.Ins.DB.Consignments.Where(x => x.Product.Type == ComboBoxCategory.Content.ToString()).OrderByDescending(x => x.ExpiryDate).Distinct().ToList());
                    FilteredList = new ObservableCollection<Products>((products).Where(x => x.Type == ComboBoxCategory.Content.ToString()).ToList());
                }
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

            CompleteReceiptCM = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ConvenienceStore.Model.Staff.Bill bill = new ConvenienceStore.Model.Staff.Bill();
            });
        }
    }
}
