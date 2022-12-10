using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class HistoryViewModel : BaseViewModel
    {
        #region Region for ICommand
        public ICommand SearchBill { get; set; }
        #endregion

        private ObservableCollection<Bills> _BillList;
        public ObservableCollection<Bills> BillList { get { return _BillList; } set { _BillList = value; OnPropertyChanged(); } }

        public HistoryViewModel()
        {
            //BillList = new ObservableCollection<Bill>(DataProvider.Ins.DB.Bills);

        }
    }
}
