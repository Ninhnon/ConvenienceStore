namespace ConvenienceStore.Model.Admin
{
    public class SalaryBill
    {
        private int id;
        public int IdBill { get => id; set => id = value; }
        private int idAccount;
        public int IdAccount { get => idAccount; set => idAccount = value; }
        private string billDate;
        public string BillDate { get => billDate; set => billDate = value; }
        private int customerId_;
        public int CustomerId { get => customerId_; set => customerId_ = value; }
        private long price;
        public long Price { get => price; set => price = value; }
        private string employeeName_;
        public string EmployeeName { get => employeeName_; set => employeeName_ = value; }
    }
}
