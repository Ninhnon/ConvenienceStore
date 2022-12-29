namespace ConvenienceStore.Model.Staff
{
    using ConvenienceStore.ViewModel.StaffVM;
    public class Bill : BaseViewModel
    {
#nullable enable
        private int? _Id;
        public int? Id { get { return _Id; } set { _Id = value; OnPropertyChanged(); } }
        private System.DateTime? _BillDate;
        public System.DateTime? BillDate { get { return _BillDate; } set { _BillDate = value; OnPropertyChanged(); } }
        private int? _CustomerId;
        public int? CustomerId { get { return _CustomerId; } set { _CustomerId = value; OnPropertyChanged(); } }
        private int? _UserId;
        public int? UserId { get { return _UserId; } set { _UserId = value; OnPropertyChanged(); } }
        private int? _Price;
        public int? Price { get { return _Price; } set { _Price = value; OnPropertyChanged(); } }
#nullable disable
    }
}
