namespace ConvenienceStore.Model.Staff
{
    using ConvenienceStore.Utils.Helpers;
    using ConvenienceStore.ViewModel.StaffVM;
    using System;

    public partial class Report : BaseViewModel
    {
        private string _Id; public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }
        private string _Title; public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }
        private string _Description; public string Description { get => _Description; set { _Description = value; OnPropertyChanged(); } }
        private string _Status; public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }
        private int _StaffId; public int StaffId { get => _StaffId; set { _StaffId = value; OnPropertyChanged(); } }
        private string _Level; public string Level { get => _Level; set { _Level = value; OnPropertyChanged(); } }
        private byte[] _Image; public byte[] Image { get => _Image; set { _Image = value; OnPropertyChanged(); } }
        public decimal? _RepairCost; public decimal? RepairCost { get => _RepairCost; set { _RepairCost = value; OnPropertyChanged(); } }
        public DateTime _SubmittedAt; public DateTime SubmittedAt { get => _SubmittedAt; set { _SubmittedAt = value; OnPropertyChanged(); } }
        public DateTime? _StartDate; public DateTime? StartDate { get => _StartDate; set { _StartDate = value; OnPropertyChanged(); } }
        public DateTime? _FinishDate; public DateTime? FinishDate { get => _FinishDate; set { _FinishDate = value; OnPropertyChanged(); } }
        public string? RepairCostStr
        {
            get
            {
                return Helpers.FormatVNMoney(RepairCost);
            }
        }
    }
}