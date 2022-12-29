using System;
using System.ComponentModel;

namespace ConvenienceStore.Model.Admin
{
    public class Product : INotifyPropertyChanged
    {
        public int InputInfoId { get; set; }
        public string Barcode { get; set; }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged("Type"); }
        }

        private string productionSite;

        public string ProductionSite
        {
            get { return productionSite; }
            set { productionSite = value; OnPropertyChanged("ProductionSite"); }
        }

        private byte[] image;

        public byte[] Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }

        private int cost;

        public int Cost
        {
            get { return cost; }
            set { cost = value; OnPropertyChanged("Cost"); }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged("Price"); }
        }
        private int total;
        public int Total
        {
            get { return total; }
            set { total = value; OnPropertyChanged("Total"); }
        }
        private int stock;

        public int Stock
        {
            get { return stock; }
            set { stock = value; OnPropertyChanged("Stock"); }
        }

        private DateTime manufacturingDate;

        public DateTime ManufacturingDate
        {
            get { return manufacturingDate; }
            set { manufacturingDate = value; OnPropertyChanged("ManufacturingDate"); }
        }

        private DateTime expiryDate;

        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; OnPropertyChanged("ExpiryDate"); }
        }

        private double discount;

        public double Discount
        {
            get { return discount; }
            set { discount = value; OnPropertyChanged("Discount"); }
        }

        private int inStock;

        public int InStock
        {
            get { return inStock; }
            set { inStock = value; OnPropertyChanged("InStock"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
