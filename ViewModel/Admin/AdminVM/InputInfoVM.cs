using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand;
using ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand.DeleteInputInfoCommand;
using ConvenienceStore.ViewModel.Admin.Command.ProductCommand;
using ConvenienceStore.ViewModel.Admin.Command.ProductCommand.AddNewProductCommand;
using ConvenienceStore.ViewModel.Admin.Command.ProductCommand.ProductCardCommand;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class InputInfoVM : BaseViewModel, INotifyPropertyChanged
    {
        private string curAccountName;

        public string CurAccountName
        {
            get { return curAccountName; }
            set
            {
                curAccountName = value;
                OnPropertyChanged("CurAccountName");
            }
        }

        public ObservableCollection<Manager> managers { get; set; }
        public List<InputInfo> inputInfos { get; set; }
        public ObservableCollection<InputInfo> ObservableInputInfos { get; set; }

        private int isDesc;

        public int IsDesc
        {

            get { return isDesc; }
            set
            {
                isDesc = value;
                OnPropertyChanged("IsDesc");

                OrderByInputDate();
            }
        }


        private Manager selectedManager;
        public Manager SelectedManager
        {

            get { return selectedManager; }
            set
            {
                selectedManager = value;
                OnPropertyChanged("SelectedManager");

                if (selectedManager != null)
                {
                    SetInputInfosCoresspondManager();
                }
            }
        }


        public List<Supplier> suppliers { get; set; }

        private InputInfo selectedInputInfo;

        public InputInfo SelectedInputInfo
        {
            get { return selectedInputInfo; }
            set
            {
                selectedInputInfo = value;
                OnPropertyChanged("SelectedInputInfo");
            }
        }

        public List<Product> products;

        private string searchContent;

        public string SearchContent
        {
            get { return searchContent; }
            set
            {
                searchContent = value;
                OnPropertyChanged("SearchContent");

                if (searchContent == "")
                {
                    SetProductsCorrespondSearch();
                }
            }
        }

        public ObservableCollection<Product> ObservableProducts { get; set; }

        private Product selectedProduct;

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        private InputInfo deletedInputInfo;

        public InputInfo DeletedInputInfo
        {
            get { return deletedInputInfo; }
            set
            {
                deletedInputInfo = value;
                OnPropertyChanged("DeletedInputInfo");
            }
        }

        public Snackbar InputInfoSnackbar;

        public Snackbar ProductSnackbar;

        // Command

        public OpenInputInfoCommand OpenInputInfoCommand { get; set; }
        public AddInputInfoButtonCommand AddInputInfoButtonCommand { get; set; }
        public CreateInputInfoButtonCommand CreateInputInfoButtonCommand { get; set; }
        public OpenAlertDialog OpenAlertDialog { get; set; }
        public DeleteInputInfoCommand DeleteInputInfoCommand { get; set; }
        public SearchButtonCommand SearchButtonCommand { get; set; }
        public AddProductButtonCommand AddProductButtonCommand { get; set; }
        public DeleteProductCommand DeleteProductCommand { get; set; }
        public SaveNewProductCommand SaveNewProductCommand { get; set; }
        public BindingProductSnackbar BindingProductSnackbar { get; set; }
        public CheckExistBarcode CheckExistBarcode { get; set; }
        public RefreshInputInfoData RefreshInputInfoData { get; set; }

        public InputInfoVM()
        {
            CurAccountName = CurrentAccount.Name;

            inputInfos = new List<InputInfo>();
            ObservableInputInfos = new ObservableCollection<InputInfo>();

            IsDesc = 0;

            managers = DatabaseHelper.FetchingManagers();
            managers.Insert(0, new Manager()
            {
                Id = 0,
                Name = "Tất cả"
            });

            suppliers = DatabaseHelper.FetchingSupplier();

            products = new List<Product>();
            ObservableProducts = new ObservableCollection<Product>();

            OpenInputInfoCommand = new OpenInputInfoCommand(this);
            AddInputInfoButtonCommand = new AddInputInfoButtonCommand(this);
            CreateInputInfoButtonCommand = new CreateInputInfoButtonCommand(this);
            OpenAlertDialog = new OpenAlertDialog(this);
            DeleteInputInfoCommand = new DeleteInputInfoCommand(this);

            SearchButtonCommand = new SearchButtonCommand(this);
            AddProductButtonCommand = new AddProductButtonCommand(this);
            DeleteProductCommand = new DeleteProductCommand(this);
            SaveNewProductCommand = new SaveNewProductCommand(this);

            BindingProductSnackbar = new BindingProductSnackbar(this);
            CheckExistBarcode = new CheckExistBarcode(this);

            RefreshInputInfoData = new RefreshInputInfoData(this);
        }

        public void LoadProducts()
        {
            products = selectedInputInfo.products;

            searchContent = "";
            SetProductsCorrespondSearch();
        }

        public void SetInputInfosCoresspondManager()
        {
            ObservableInputInfos.Clear();

            if (isDesc == 1)
            {
                for (int i = inputInfos.Count - 1; i >= 0; --i)
                {
                    if (inputInfos[i].UserId == selectedManager.Id || selectedManager.Name == "Tất cả")
                    {
                        ObservableInputInfos.Add(inputInfos[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < inputInfos.Count; ++i)
                {
                    if (inputInfos[i].UserId == selectedManager.Id || selectedManager.Name == "Tất cả")
                    {
                        ObservableInputInfos.Add(inputInfos[i]);
                    }
                }
            }

        }

        public void OrderByInputDate()
        {
            if (isDesc == 1)
            {
                var descInputInfos = ObservableInputInfos.OrderByDescending(e => e.InputDate).ToList();
                ObservableInputInfos.Clear();
                for (int i = 0; i < descInputInfos.Count; ++i)
                {
                    ObservableInputInfos.Add(descInputInfos[i]);
                }
            }
            else
            {
                var ascInputInfos = ObservableInputInfos.OrderBy(e => e.InputDate).ToList();
                ObservableInputInfos.Clear();
                for (int i = 0; i < ascInputInfos.Count; ++i)
                {
                    ObservableInputInfos.Add(ascInputInfos[i]);
                }
            }

        }

        public void SetProductsCorrespondSearch()
        {
            ObservableProducts.Clear();

            for (int i = 0; i < products.Count; ++i)
            {
                if (products[i].Barcode.Contains(searchContent) || products[i].Title.ToLower().Contains(searchContent.ToLower()))
                {
                    ObservableProducts.Add(products[i]);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
