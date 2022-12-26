using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model;
using ConvenienceStore.Utils.Helpers;
using System.Windows.Input;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Views.Admin;
using System.Windows.Controls;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class ProfileViewModel : BaseViewModel
    {
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        private byte[] _Avatar;

        public byte[] Avatar
        {
            get { return _Avatar; }
            set { _Avatar = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Member> MyTeam { get; set; }
        public ICommand LoadCommand { get; set; }

        public ProfileViewModel()
        {

            ManagerId = CurrentAccount.ManagerId;
            Name = CurrentAccount.Name;
            Address = CurrentAccount.Address;
            Email = CurrentAccount.Email;
            Phone = CurrentAccount.Phone;
            Avatar = CurrentAccount.Avatar;
            MyTeam = DatabaseHelper.QueryStaffOnTeam(ManagerId);
            LoadCommand = new RelayCommand<Page>((p) =>
            {
                return true;
            }, (p) =>
            {
                Load(p);
            });
        }

        public void Load(Page parameter)
        {
            parameter.DataContext = new ProfileViewModel();
        }
    }
}
