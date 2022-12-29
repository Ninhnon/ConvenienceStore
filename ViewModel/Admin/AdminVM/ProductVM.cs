using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.Command.SmallProductCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class ProductVM : INotifyPropertyChanged
    {
        public List<SmallProduct> smallProducts { get; set; }

        public ObservableCollection<SmallProduct> ObservableSmallProducts { get; set; }

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
                    SetBlockSmallProductCorespondSearch();
                }
            }
        }

        private SmallProduct selectedSmallProduct;

        public SmallProduct SelectedSmallProduct
        {
            get { return selectedSmallProduct; }
            set
            {
                selectedSmallProduct = value;
                OnPropertyChanged("SelectedSmallProduct");
            }
        }

        public RefreshSmallProductData RefreshSmallProductData { get; set; }

        public ProductVM()
        {
            smallProducts = new List<SmallProduct>();
            ObservableSmallProducts = new ObservableCollection<SmallProduct>();

            RefreshSmallProductData = new RefreshSmallProductData(this);
        }

        public void SetBlockSmallProductCorespondSearch()
        {
            ObservableSmallProducts.Clear();

            for (int i = 0; i < smallProducts.Count; ++i)
            {
                if (smallProducts[i].Title.ToLower().Contains(searchContent.ToLower()) || smallProducts[i].Barcode.Contains(searchContent))
                {
                    ObservableSmallProducts.Add(smallProducts[i]);
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
