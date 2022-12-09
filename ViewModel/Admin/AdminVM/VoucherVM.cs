using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    class VoucherVM : INotifyPropertyChanged
    {
        public List<BlockVoucher> blockVouchers {get; set;}

        public ObservableCollection<BlockVoucher> observableBlockVouchers { get; set;}

        private string searchContent;

        public string SearchContent
        {
            get { return searchContent; }
            set
            {
                searchContent = value;
                OnPropertyChanged("SearchContent");

                if (searchContent == "")
                {
                    SetBlockVoucherCorespondSearch();
                }
            }
        }

        private BlockVoucher selectedBlockVoucher;

        public BlockVoucher SelectedBlockVoucher
        {
            get { return selectedBlockVoucher; }
            set
            {
                selectedBlockVoucher = value;
                OnPropertyChanged("SelectedBlockVoucher");
            }
        }

        public GenerateBlockVoucher GenerateBlockVoucher { get; set; }

        public ObservableCollection<Voucher> vouchers { get; set;}
        public VoucherVM()
        {
            blockVouchers = DatabaseHelper.FetchingBlockVoucherData();

            observableBlockVouchers = new ObservableCollection<BlockVoucher>();
            for (int i = 0; i < blockVouchers.Count; i++)
            {
                observableBlockVouchers.Add(blockVouchers[i]);
            }

            vouchers = new ObservableCollection<Voucher>();

            GenerateBlockVoucher = new GenerateBlockVoucher(this);
        }

        public void SetBlockVoucherCorespondSearch()
        {
            observableBlockVouchers.Clear();

            for (int i = 0; i < blockVouchers.Count; ++i)
            {
                if (blockVouchers[i].ReleaseName.ToLower().Contains(searchContent.ToLower()))
                {
                    observableBlockVouchers.Add(blockVouchers[i]);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
