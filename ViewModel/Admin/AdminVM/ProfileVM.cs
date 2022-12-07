﻿using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    class ProfileVM : INotifyPropertyChanged
    {
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        private byte[] avatar;

        public byte[] Avatar
        {
            get { return avatar; }
            set { avatar = value; OnPropertyChanged("Avatar"); }
        }

        public ObservableCollection<Member> MyTeam { get; set; }

        public ProfileVM()
        {
            ManagerId = CurrentAccount.idAccount;
            Name = CurrentAccount.Name; 
            Address = CurrentAccount.Address;
            Email = CurrentAccount.Email;
            Phone = CurrentAccount.Phone;
            Avatar = CurrentAccount.Avatar;
            MyTeam = DatabaseHelper.QueryStaffOnTeam(CurrentAccount.idAccount);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
