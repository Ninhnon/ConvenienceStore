using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ICommand AddToCartBarCode { get; set; }
        public ICommand IncreaseProductAmount { get; set; }
        public ICommand DecreaseProductAmount { get; set; }
        public ICommand RemoveProduct { get; set; }
        public ICommand SearchProductName { get; set; }
        public ICommand FilterType { get; set; }

        public ICommand MaskNameCM { get; set; }
        public ICommand OpenReceiptPage { get; set; }
        public ICommand ScrollToEndListBox { get; set; }
        public ICommand OpenBarCodeCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand FindProductCommand { get; set; }

        #endregion

        #region Icommand Receipt
        public ICommand LoadReceiptPage { get; set; }
        public ICommand CancelReceiptCM { get; set; }
        public ICommand PrintBillCM { get; set; }
        public ICommand SearchCustomerIdCM { get; set; }
        public ICommand SearchVoucherCM { get; set; }
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

        private int _Discount;
        public int Discount { get { return _Discount; } set { _Discount = value; OnPropertyChanged(); } }

        private string _VoucherCode;
        public string VoucherCode { get { return _VoucherCode; } set { _VoucherCode = value; OnPropertyChanged(); } }

        private int? _CustomerId;
        public int? CustomerId { get { return _CustomerId; } set { _CustomerId = value; OnPropertyChanged(); } }

        //2 thuộc tính để quản lý việc lost focus
        private string _PrevVoucherCode;
        public string PrevVoucherCode { get { return _PrevVoucherCode; } set { _PrevVoucherCode = value; OnPropertyChanged(); } }

        private int? _PrevCustomerId;
        public int? PrevCustomerId { get { return _PrevCustomerId; } set { _PrevCustomerId = value; OnPropertyChanged(); } }

        private Receipt _ReceiptPage;
        public Receipt ReceiptPage { get { return _ReceiptPage; } set { _ReceiptPage = value; OnPropertyChanged(); } }

        //public Receipt ReceiptPage

        public List<Products> products = new List<Products>();
        public List<Customer> customers = new List<Customer>();
        public List<ConvenienceStore.Model.Staff.Bill> bill = new List<ConvenienceStore.Model.Staff.Bill>();

        private Model.Staff.Bill? _ReceiptBill;
        public Model.Staff.Bill? ReceiptBill { get { return _ReceiptBill; } set { _ReceiptBill = value; OnPropertyChanged(); } }

        #region Staff Info
        private int _StaffId;
        public int StaffId { get { return _StaffId; } set { _StaffId = value; OnPropertyChanged(); } }

        private string _StaffName;
        public string StaffName { get { return _StaffName; } set { _StaffName = value; OnPropertyChanged(); } }
        #endregion

        public void AddBarCode(BarCodeUC parameter)
        {
            SearchContent = parameter.txtBarcode.Text;

        }
        public void ShowBarCodeQR(PaymentWindow parameter)
        {

            parameter.barcodeUC.Visibility = Visibility.Visible;
        }
        public void FindProduct(BarCodeUC parameter)
        {
            FilteredList = List;
            if (parameter.txtBarcode.Text != "")
            {
                if (long.TryParse(parameter.txtBarcode.Text, out long n))
                {
                    FilteredList = new ObservableCollection<Products>(FilteredList.Where(x => x.BarCode.ToLower().Contains(parameter.txtBarcode.Text.ToLower())).ToList());
                }
                else
                {
                    FilteredList = new ObservableCollection<Products>(FilteredList.Where(x => x.Title.ToLower().Contains(parameter.txtBarcode.Text.ToLower())).ToList());
                }
                //Kiểm tra trong giỏ hàng đã có hay chưa, có rồi thì không thêm vào
                if (FilteredList.Count > 0)
                {
                    SelectedItem = FilteredList[0];
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

                        TotalBill += (int)billDetail.TotalPrice;
                        SelectedBillDetail = billDetail;
                        ShoppingCart.Add(billDetail);
                    }
                }

            }
        }

        public PaymentViewModel()
        {

            StaffName = CurrentAccount.Name;
            StaffId = CurrentAccount.idAccount;
            products = DatabaseHelper.FetchingProductData();
            List = new ObservableCollection<Products>(products);
            FilteredList = List;
            ShoppingCart = new ObservableCollection<BillDetails>();
            TotalBill = 0;

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

                     TotalBill += (int)billDetail.TotalPrice;
                     SelectedBillDetail = billDetail;
                     ShoppingCart.Add(billDetail);
                 }
             }
            );
            AddToCartBarCode = new RelayCommand<BarCodeUC>(parameter => true, parameter => AddBarCode(parameter));
            OpenBarCodeCommand = new RelayCommand<PaymentWindow>(parameter => true, parameter => ShowBarCodeQR(parameter));
            FindProductCommand = new RelayCommand<BarCodeUC>(parameter => true, parameter => FindProduct(parameter));

            //Tăng giảm số lượng, xóa khỏi giỏ hàng
            IncreaseProductAmount = new RelayCommand<BillDetail>((p) =>
            {
                return true;
            }, (p) =>
            {
                BillDetails item = SelectedBillDetail;
                if (item != null)
                {
                    TotalBill -= (int)item.TotalPrice;
                    item.TotalPrice = item.TotalPrice / item.Quantity * (item.Quantity + 1);
                    TotalBill += (int)item.TotalPrice;
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
                        TotalBill -= (int)item.TotalPrice;
                        item.TotalPrice = item.TotalPrice / item.Quantity * (item.Quantity - 1);
                        TotalBill += (int)item.TotalPrice;
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
                    TotalBill -= (int)item.TotalPrice;
                }
            }
            );

            //Tìm kiếm & lọc
            SearchProductName = new RelayCommand<TextBox>((p) =>
            {
                return true;
            },


            (p) =>
            {
                TextBox? tbx = p;

                FilteredList = List;
                if (tbx.Text != "")
                    if (long.TryParse(tbx.Text, out long n))
                    {
                        FilteredList = new ObservableCollection<Products>(FilteredList.Where(x => x.BarCode.ToLower().Contains(tbx.Text.ToLower())).ToList());
                    }
                    else
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
                //Receipt wd = new Receipt();
                ReceiptPage = new Receipt();
                MaskName.Visibility = Visibility.Visible;

                TotalBill = 0;
                Discount = 0;

                foreach (BillDetails bd in ShoppingCart)
                {
                    TotalBill += (int)bd.TotalPrice;
                }

                ReceiptPage.ShowDialog();
                //wd.ShowDialog();
            });

            CancelReceiptCM = new RelayCommand<Button>((p) =>
            {
                return true;
            }, (p) =>
            {
                // Nếu đã thanh toán xong thì làm trống giỏ hàng

                if (p.Content.ToString() == "Thoát")
                {
                    while (ShoppingCart.Count > 0)
                    {
                        ShoppingCart.RemoveAt(ShoppingCart.Count - 1);
                    }
                }

                ReceiptPage.Close();
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
                try
                {
                    if (PrevCustomerId == CustomerId && CustomerId != null) //Xử lý lost focus
                        return;

                    if (p.Text == "")
                    {
                        PrevCustomerId = CustomerId = null;
                        return;
                    }

                    customers = DatabaseHelper.FetchingCustomerData();
                    var checkCustomerId = customers.Where(x => Convert.ToString(x.Id) == p.Text);

                    if (checkCustomerId.Count() == 1)   //Kiểm tra mã khách hàng hợp lệ
                    {
                        if (CustomerId != Convert.ToInt32(p.Text))   //Xử lí việc nhập cùng 1 mã nhiều lần
                            PrevCustomerId = CustomerId;
                        else
                            return;
                        CustomerId = Convert.ToInt32(p.Text);
                        MessageBoxCustom mbSuccess = new MessageBoxCustom("Thông báo", "Mã khách hàng hợp lệ", MessageType.Success, MessageButtons.OK);
                        mbSuccess.ShowDialog();
                    }
                    else
                    {
                        p.Text = null;
                        CustomerId = null;
                        MessageBoxCustom mbFailed = new MessageBoxCustom("Cảnh báo", "Mã khách hàng không hợp lệ", MessageType.Warning, MessageButtons.OK);
                        mbFailed.ShowDialog();
                    }
                }
                catch
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Có lỗi xảy ra, vui lòng thử lại", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            });

            SearchVoucherCM = new RelayCommand<TextBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    if (p.Text != "")
                    {
                        //Xử lý lost focus
                        if (VoucherCode == p.Text)
                            return;
                        if (PrevVoucherCode == VoucherCode && VoucherCode != null) //Xử lí lost focus
                            return;
                        //Lấy lại TotalBill
                        //Tạo một biến int trả về mã lỗi
                        //0: Mã giảm giá không tồn tại hoặc đã được sử dụng
                        //1: Mã giảm giá hết hạn
                        MessageBoxCustom mb;

                        int err = -1;
                        TotalBill += Discount;
                        Discount = DatabaseHelper.ApplyVoucher(TotalBill, p.Text, ref err);

                        if (err == 0)
                        {
                            mb = new MessageBoxCustom("Áp dụng mã giảm giá thất bại", "Mã giảm giá không tồn tại hoặc đã được sử dụng", MessageType.Warning, MessageButtons.OK);
                            mb.ShowDialog();
                            p.Text = "";
                        }
                        else if (err == 1)
                        {
                            mb = new MessageBoxCustom("Áp dụng mã giảm giá thất bại", "Mã giảm giá đã quá hạn sử dụng, vui lòng thử lại với mã giảm giá khác", MessageType.Warning, MessageButtons.OK);
                            mb.ShowDialog();
                            p.Text = "";
                        }
                        else
                        {
                            PrevVoucherCode = VoucherCode;
                            VoucherCode = p.Text;

                            //Xử lí trường hợp nhập nhiều mã voucher
                            if (TotalBill - Discount < 0)
                                TotalBill = 0;
                            else
                                TotalBill -= Discount;
                            mb = new MessageBoxCustom("Áp dụng mã giảm giá thành công", "Hóa đơn sẽ được giảm giá khi thanh toán thành công", MessageType.Success, MessageButtons.OK);
                            mb.ShowDialog();
                        }
                    }
                    else
                        return;
                }
                catch
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Cảnh báo", "Có lỗi xảy ra, vui lòng thử lại", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            });

            CompleteReceiptCM = new RelayCommand<Button>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    //Xử lý phần tổng hóa đơn khi có áp dụng voucher
                    DatabaseHelper.InsertBill(CustomerId, TotalBill, Discount);
                    ReceiptBill = DatabaseHelper.FetchingBillData().LastOrDefault();
                    if (ReceiptBill == null)
                        return;

                    //Sửa thông tin BillId trong các BillDetail và thêm cái BillDetail vào database
                    foreach (BillDetails bd in ShoppingCart)
                    {
                        bd.BillId = ReceiptBill.Id;
                        DatabaseHelper.InsertBillDetail(bd);
                    }

                    //Sửa trạng thái voucher trong database
                    DatabaseHelper.UpdateVoucherStatus(VoucherCode);
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", "Thanh toán thành công", MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();

                    //Disable nút thanh toán
                    p.IsEnabled = false;
                }
                catch
                {
                    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", "Thanh toán gặp sự cố, vui lòng thử lại!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                }
            });
        }
    }
}
