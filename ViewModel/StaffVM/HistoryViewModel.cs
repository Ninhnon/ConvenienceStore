using ConvenienceStore.Model;
using ConvenienceStore.ViewModel.MainBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class HistoryViewModel : BaseViewModel
    {
        #region Region for ICommand

        #endregion
        ObservableCollection<Bill> _BillList;
        public ObservableCollection<Bill> BillList { get { return _BillList; } set { _BillList = value; OnPropertyChanged(); } }

        private Bill _SelectedItem;
        public Bill SelectedItem { get { return _SelectedItem; } set { _SelectedItem = value; OnPropertyChanged(); } }

        //public HistoryViewModel()
        //{
        //    BillList = new ObservableCollection<Bill>(DataProvider.Ins.DB.Bills);
        //}
    }
}
