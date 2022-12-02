using ConvenienceStore.Model.Lam;
using ConvenienceStore.ViewModel.Lam.Command.InputInfoCommand;
using ConvenienceStore.ViewModel.Lam.Command.ProductCommand;
using ConvenienceStore.ViewModel.Lam.Command.ProductCommand.AddNewProductCommand;
using ConvenienceStore.ViewModel.Lam.Command.ProductCommand.ProductCardCommand;
using ConvenienceStore.ViewModel.Lam.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.ViewModel.Lam
{
    public class InputInfoVM : INotifyPropertyChanged
    {
        public ObservableCollection<Manager> managers { get; set; }
        public List<InputInfo> inputInfos { get; set; }
        public ObservableCollection<InputInfo> ObservableInputInfos { get; set; }

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


        public ObservableCollection<Supplier> suppliers { get; set; }

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

        // Command

        public OpenInputInfoCommand OpenInputInfoCommand { get; set; }
        public AddInputInfoButtonCommand AddInputInfoButtonCommand { get; set; }
        public CreateInputInfoButtonCommand CreateInputInfoButtonCommand { get; set; }
        public DeleteInputInfoCommand DeleteInputInfoCommand { get; set; }
        public SearchButtonCommand SearchButtonCommand { get; set; }
        public AddProductButtonCommand AddProductButtonCommand { get; set; }
        public DeleteProductCommand DeleteProductCommand { get; set; }
        public SaveNewProductCommand SaveNewProductCommand { get; set; }
        public EditProductButton EditProductButton { get; set; }

        public InputInfoVM()
        {
            inputInfos = DatabaseHelper.FetchingInputInfo();
            ObservableInputInfos = new ObservableCollection<InputInfo>();

            managers = DatabaseHelper.FetchingManagers();
            managers.Insert(0, new Manager()
            {
                Id = 0,
                Name = "All"
            });

            suppliers = DatabaseHelper.FetchingSupplier();

            products = new List<Product>();
            ObservableProducts = new ObservableCollection<Product>();

            OpenInputInfoCommand = new OpenInputInfoCommand(this);
            AddInputInfoButtonCommand = new AddInputInfoButtonCommand(this);
            CreateInputInfoButtonCommand = new CreateInputInfoButtonCommand(this);
            DeleteInputInfoCommand = new DeleteInputInfoCommand(this);

            SearchButtonCommand = new SearchButtonCommand(this);
            AddProductButtonCommand = new AddProductButtonCommand(this);
            DeleteProductCommand = new DeleteProductCommand(this);
            SaveNewProductCommand = new SaveNewProductCommand(this);

            EditProductButton = new EditProductButton(this);

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

            for (int i = 0; i < inputInfos.Count; ++i)
            {
                if (inputInfos[i].UserId == selectedManager.Id || selectedManager.Name == "All")
                {
                    ObservableInputInfos.Add(inputInfos[i]);
                }
            }
        }

        public void SetProductsCorrespondSearch()
        {
            ObservableProducts.Clear();

            for (int i = 0; i < products.Count; ++i)
            {
                if (products[i].Barcode.Contains(searchContent))
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
