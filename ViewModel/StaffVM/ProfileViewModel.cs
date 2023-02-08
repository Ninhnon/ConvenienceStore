using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class ProfileViewModel : BaseViewModel
    {
        public int Id { get; set; }
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
            Id = CurrentAccount.idAccount;
            ManagerId = CurrentAccount.ManagerId;
            Name = CurrentAccount.Name;
            Address = CurrentAccount.Address;
            Email = CurrentAccount.Email;
            Phone = CurrentAccount.Phone;
            Avatar = CurrentAccount.Avatar;
            MyTeam = DatabaseHelper.FetchTeamMembers(Id, ManagerId);

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
