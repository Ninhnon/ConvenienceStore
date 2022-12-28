using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Admin.ProductWindow;
using ConvenienceStore.Views.Login;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class ProductVM : BaseViewModel
    {

        SoundPlayer player = new SoundPlayer(@"..\..\..\beep.wav");
        public ICommand AddToCartBarCode { get; set; }
        public ICommand SearchProductName { get; set; }
        public ICommand FilterType { get; set; }

        public ICommand MaskNameCM { get; set; }
        public ICommand OpenBarCodeCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand FindProductCommand { get; set; }
        public ICommand ThrowProduct { get; set; }



        // PaymentView
        private ObservableCollection<Products> _List;
        public ObservableCollection<Products> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Products> _FilteredList;
        public ObservableCollection<Products> FilteredList { get { return _FilteredList; } set { _FilteredList = value; OnPropertyChanged(); } }

        private Products _SelectedItem;
        public Products SelectedItem { get { return _SelectedItem; } set { _SelectedItem = value; OnPropertyChanged(); } }


        private string _SearchContent;
        public string SearchContent { get { return _SearchContent; } set { _SearchContent = value; OnPropertyChanged(); } }

        private ComboBoxItem _ComboBoxCategory;
        public ComboBoxItem ComboBoxCategory { get { return _ComboBoxCategory; } set { _ComboBoxCategory = value; OnPropertyChanged(); } }

        public static Grid MaskName { get; set; }
        //public Receipt ReceiptPage

        public List<Products> products = new List<Products>();


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

        public void ShowBarCodeQR(ProductWindow parameter)
        {
            parameter.barcodeUC.Visibility = Visibility.Visible;
        }

        public void FindProduct(BarCodeUC parameter)
        {

            FilteredList = List;
            if (parameter.txtBarcode.Text != "")
            {
                player.Play();

                if (long.TryParse(parameter.txtBarcode.Text, out long n))
                {
                    FilteredList = new ObservableCollection<Products>(FilteredList.Where(x => x.BarCode.ToLower().Contains(parameter.txtBarcode.Text.ToLower())).ToList());
                }
                else
                {
                    FilteredList = new ObservableCollection<Products>(FilteredList.Where(x => x.Title.ToLower().Contains(parameter.txtBarcode.Text.ToLower())).ToList());
                }
            }
        }

        public ProductVM()
        {

            StaffName = CurrentAccount.Name;
            StaffId = CurrentAccount.idAccount;

            FilteredList = List;
            AddToCartBarCode = new RelayCommand<BarCodeUC>(parameter => true, parameter => AddBarCode(parameter));
            OpenBarCodeCommand = new RelayCommand<ProductWindow>(parameter => true, parameter => ShowBarCodeQR(parameter));

            LoadCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                products = DatabaseHelper.FetchingProductDataT();
                List = new ObservableCollection<Products>(products);
                FilteredList = List;
            });

            FindProductCommand = new RelayCommand<BarCodeUC>(parameter => true, parameter => FindProduct(parameter));
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
            ThrowProduct = new RelayCommand<DataGrid>((p) =>
            {
                return true;
            }, (p) =>
            {
                var x = SelectedItem;
                MessageBoxCustom mb = new MessageBoxCustom("Xử lý sản phẩm", "Bạn có chắc muốn xử lý sản phẩm ra khỏi kho?", MessageType.Info, MessageButtons.YesNo);

                if (mb.ShowDialog() == true)
                {
                    List.Remove(SelectedItem);
                    DatabaseHelper.Throw(1, "1");
                    
                    mb.Close();
                }
            });

        }

    }
}
