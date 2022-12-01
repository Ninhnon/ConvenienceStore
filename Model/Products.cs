using CinemaManagement.Utils;
using ConvenienceStore.ViewModel.MainBase;
using System;

namespace ConvenienceStore.Model
{
    public partial class Products : BaseViewModel
    {
        public string _BarCode = ""; public string BarCode { get => _BarCode; set { _BarCode = value; OnPropertyChanged(); } }
        public string _Title = ""; public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }
        public string _ProductionSite = ""; public string ProductionSite { get => _ProductionSite; set { _ProductionSite = value; OnPropertyChanged(); } }
        private Byte[]? _Image; public Byte[]? Image { get => _Image; set { _Image = value; OnPropertyChanged(); } }
        public int _Cost; public int Cost { get => _Cost; set { _Cost = value; OnPropertyChanged(); } }
        public int _Price; public int Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }
        public int _Stock; public int Stock { get => _Stock; set { _Stock = value; OnPropertyChanged(); } }
        public System.DateTime _ManufacturingDate; public System.DateTime ManufacturingDate { get => _ManufacturingDate; set { _ManufacturingDate = value; OnPropertyChanged(); } }
        public System.DateTime _ExpiryDate; public System.DateTime ExpiryDate { get => _ExpiryDate; set { _ExpiryDate = value; OnPropertyChanged(); } }
        public int? _Discount; public int? Discount { get => _Discount; set { _Discount = value; OnPropertyChanged(); } }
        public string? _Type = ""; public string? Type { get => _Type; set { _Type = value; OnPropertyChanged(); } }

        public string HSDColor
        {
            get
            {
                return Helper.HSDStr(ExpiryDate);
            }
        }
    }
}