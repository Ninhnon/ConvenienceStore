namespace ConvenienceStore.Model
{
    public class CurrentAccount
    {
        private static int idAccount_;
        public static int idAccount
        {
            get => idAccount_;
            set { idAccount_ = value; }
        }
        private static string userRole_;
        public static string UserRole
        {
            get => userRole_;
            set { userRole_ = value; }
        }
        private static string name_;
        public static string Name
        {
            get => name_;
            set { name_ = value; }
        }
        private static string address_;
        public static string Address
        {
            get { return address_; }
            set { address_ = value; }
        }
        private static string phone_;
        public static string Phone
        {
            get { return phone_; }
            set { phone_ = value; }
        }
        private static string email_;
        public static string Email
        {
            get { return email_; }
            set
            {
                email_ = value;
            }
        }

        private static string username_;
        public static string UserName
        {
            get => username_;
            set { username_ = value; }
        }
        private static string password_;
        public static string Password
        {
            get => password_;
            set { password_ = value; }
        }
        private static byte[] avatar_;
        public static byte[] Avatar
        {
            get => avatar_;
            set { avatar_ = value; }
        }

        private static int managerId_;
        public static int ManagerId
        { get { return managerId_; } set { managerId_ = value; } }

    }
}

