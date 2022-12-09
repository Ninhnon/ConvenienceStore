using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
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

        public ObservableCollection<BlockVoucher> ObservableBlockVouchers { get; set;}

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

        public List<Voucher> vouchers { get; set; }
        public ObservableCollection<Voucher> ObservableVouchers { get; set; }

        public GenerateBlockVoucher GenerateBlockVoucher { get; set; }
        public OpenVoucherCommand OpenVoucherCommand { get; set; }

        public VoucherVM()
        {
            blockVouchers = DatabaseHelper.FetchingBlockVoucherData();

            ObservableBlockVouchers = new ObservableCollection<BlockVoucher>();
            for (int i = 0; i < blockVouchers.Count; i++)
            {
                ObservableBlockVouchers.Add(blockVouchers[i]);
            }

            vouchers = new List<Voucher>();
            ObservableVouchers = new ObservableCollection<Voucher>();

            GenerateBlockVoucher = new GenerateBlockVoucher(this);
            OpenVoucherCommand = new OpenVoucherCommand(this);
        }

        public void SetBlockVoucherCorespondSearch()
        {
            ObservableBlockVouchers.Clear();

            for (int i = 0; i < blockVouchers.Count; ++i)
            {
                if (blockVouchers[i].ReleaseName.ToLower().Contains(searchContent.ToLower()))
                {
                    ObservableBlockVouchers.Add(blockVouchers[i]);
                }
            }
        }
        public void LoadVouchers()
        {
            vouchers = selectedBlockVoucher.vouchers;
            ObservableVouchers.Clear();
            for (int i = 0; i < vouchers.Count; ++i)
            {
                ObservableVouchers.Add(vouchers[i]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
