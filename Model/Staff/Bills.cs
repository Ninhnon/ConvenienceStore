using ConvenienceStore.ViewModel.StaffVM;


namespace ConvenienceStore.Model.Staff
{
    public class Bills : BaseViewModel
    {
#nullable enable

        private int? _BillId;
        public int? BillId { get { return _BillId; } set { _BillId = value; OnPropertyChanged(); } }

        private string? _UserName;
        public string? UserName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }

        private string? _CustomerName;
        public string? CustomerName { get { return _CustomerName; } set { _CustomerName = value; OnPropertyChanged(); } }

        private System.DateTime? _BillDate;
        public System.DateTime? BillDate { get { return _BillDate; } set { _BillDate = value; OnPropertyChanged(); } }

        private int? _TotalPrice;
        public int? TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; OnPropertyChanged(); } }

        private int? _Discount;
        public int? Discount { get { return _Discount; } set { _Discount = value; OnPropertyChanged(); } }

        private int? _UserId;
        public int? UserId { get { return _UserId; } set { _UserId = value; OnPropertyChanged(); } }

        private int? _CustomerId;
        public int? CustomerId { get { return _CustomerId; } set { _CustomerId = value; OnPropertyChanged(); } }

#nullable disable
    }
}
