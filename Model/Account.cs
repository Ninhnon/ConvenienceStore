namespace ConvenienceStore.Model
{
    public class Account
    {
        private int number_;
        public int Number
        {
            get { return number_; }
            set { number_ = value; }
        }
        private int _idAccount;
        public int IdAccount
        {
            get { return _idAccount; }
            set { _idAccount = value; }
        }
        private string userRole_;
        public string UserRole
        {
            get => userRole_;
            set { userRole_ = value; }
        }
        private string name_;
        public string Name
        {
            get => name_;
            set { name_ = value; }
        }
        private string address_;
        public string Address
        {
            get { return address_; }
            set { address_ = value; }
        }
        private string phone_;
        public string Phone
        {
            get { return phone_; }
            set { phone_ = value; }
        }
        private string email_;
        public string Email
        {
            get { return email_; }
            set
            {
                email_ = value;
            }
        }

        private string username_;
        public string UserName
        {
            get => username_;
            set { username_ = value; }
        }
        private string password_;
        public string Password
        {
            get => password_;
            set { password_ = value; }
        }
        private byte[] avatar_;
        public byte[] Avatar
        {
            get => avatar_;
            set { avatar_ = value; }
        }
        private int managerId_;
        public int ManagerId
        { get { return managerId_; } set { managerId_ = value; } }

        private int salary;
        public int Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        public Account()
        {

        }
        private int tong_;
        public int Tong
        {
            get { return tong_; }
            set { tong_ = value; }
        }

        public Account(int idAccount_, string userRole_, string name_, string address_, string phone_, string email_, string username_, string password_, byte[] avatar_, int managerId_)
        {
            this._idAccount = idAccount_;
            this.userRole_ = userRole_;
            this.name_ = name_;
            this.address_ = address_;
            this.phone_ = phone_;
            this.email_ = email_;
            this.username_ = username_;
            this.password_ = password_;
            this.avatar_ = avatar_;
            this.managerId_ = managerId_;
        }

        public Account(string userRole_, string name_, string address_, string phone_, string email_, string username_, string password_, byte[] avatar_, int managerId_)
        {

            this.userRole_ = userRole_;
            this.name_ = name_;
            this.address_ = address_;
            this.phone_ = phone_;
            this.email_ = email_;
            this.username_ = username_;
            this.password_ = password_;
            this.avatar_ = avatar_;
            this.managerId_ = managerId_;

        }
        public Account(int idAccount, string name_, int tong_)
        {
            this._idAccount = idAccount;
            this.name_ = name_;
            this.tong_ = tong_;
        }
    }
}
