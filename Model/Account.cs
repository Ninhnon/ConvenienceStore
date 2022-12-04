using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model
{
    public class Account
    {
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
        public byte[] avatar
        {
            get => avatar_;
            set { avatar_ = value; }
        }
        public Account()
        {

        }
        public Account( string userRole_, string name_, string address_, string phone_, string email_, string username_, string password_, byte[] avatar_ )
        {

            this.userRole_=userRole_;
            this.name_=name_;
            this.address_=address_;
            this.phone_=phone_;
            this.email_=email_;
            this.username_=username_;
            this.password_=password_;
            this.avatar_=avatar_;
           
        }
        public Account(int idAccount_,string userRole_, string name_, string address_, string phone_, string email_, string username_, string password_, byte[] avatar_)
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

        }
        public Account(string userRole_, string name_, string address_, string phone_, string email_, string username_, string password_)
        {

            this.userRole_ = userRole_;
            this.name_ = name_;
            this.address_ = address_;
            this.phone_ = phone_;
            this.email_ = email_;
            this.username_ = username_;
            this.password_ = password_;
            

        }
    }
}
