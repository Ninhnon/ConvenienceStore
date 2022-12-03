using ConvenienceStore.ViewModel.MainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model
{
    public partial class BillDetails : BaseViewModel
    {
        private int? _BillId;
        public int? BillId { get { return _BillId; } set { _BillId = value; OnPropertyChanged(); } }
        private string? _ProductId;
        public string? ProductId { get { return _ProductId; } set { _ProductId = value; OnPropertyChanged(); } }
        private int? _Quantity;
        public int? Quantity { get { return _Quantity; } set { _Quantity = value; OnPropertyChanged(); } }
        private int? _TotalPrice;
        public int? TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; OnPropertyChanged(); } }
        public string? _Title;
        public string? Title { get { return _Title; } set { _Title = value; OnPropertyChanged(); } }
        private Byte[]? _Image;
        public Byte[]? Image { get { return _Image; } set { _Image = value; OnPropertyChanged(); } }
    }
}
