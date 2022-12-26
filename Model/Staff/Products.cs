using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.StaffVM;
using System;

namespace ConvenienceStore.Model.Staff
{
    public partial class Products : BaseViewModel
    {
        public int _InputInfoId; public int InputInfoId { get => _InputInfoId; set { _InputInfoId = value; OnPropertyChanged(); } }
        public string _BarCode = ""; public string BarCode { get => _BarCode; set { _BarCode = value; OnPropertyChanged(); } }
        public string _Title = ""; public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }
        public string _ProductionSite = ""; public string ProductionSite { get => _ProductionSite; set { _ProductionSite = value; OnPropertyChanged(); } }
        private byte[]? _Image; public byte[]? Image { get => _Image; set { _Image = value; OnPropertyChanged(); } }
        public int _Cost; public int Cost { get => _Cost; set { _Cost = value; OnPropertyChanged(); } }
        public int _Price; public int Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }
        public int _Stock; public int Stock { get => _Stock; set { _Stock = value; OnPropertyChanged(); } }
        public DateTime _ManufacturingDate; public DateTime ManufacturingDate { get => _ManufacturingDate; set { _ManufacturingDate = value; OnPropertyChanged(); } }
        public DateTime _ExpiryDate; public DateTime ExpiryDate { get => _ExpiryDate; set { _ExpiryDate = value; OnPropertyChanged(); } }
        public double? _Discount; public double? Discount { get => _Discount; set { _Discount = value; OnPropertyChanged(); } }
        public string? _Type = ""; public string? Type { get => _Type; set { _Type = value; OnPropertyChanged(); } }

        public string HSDColor
        {
            get
            {
                return Helpers.HSDStr(ExpiryDate);
            }
        }
    }
}