using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int discount;

        public int Discount
        {
            get { return discount; }
            set { discount = value; OnPropertyChanged("Discount"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
