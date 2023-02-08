using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.Command.VoucherCommand;
using ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand;
using ConvenienceStore.ViewModel.Admin.Command.VoucherCommand.BlockVoucherCommand.DeleteBlockVoucherCommand;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    class VoucherVM : INotifyPropertyChanged
    {
        public List<BlockVoucher> blockVouchers { get; set; }

        public ObservableCollection<BlockVoucher> ObservableBlockVouchers { get; set; }

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

        private BlockVoucher deletedBlockVoucher;

        public BlockVoucher DeletedBlockVoucher
        {
            get { return deletedBlockVoucher; }
            set
            {
                deletedBlockVoucher = value;
                OnPropertyChanged("DeletedBlockVoucher");
            }
        }

        public Snackbar VoucherSnackbar;

        // Command Region
        public GenerateBlockVoucher GenerateBlockVoucher { get; set; }
        public OpenAlertDialog OpenAlertDialog { get; set; }
        public DeleteBlockVoucherButton DeleteBlockVoucherButton { get; set; }
        public OpenVoucherCommand OpenVoucherCommand { get; set; }
        public FilterActiveVoucherCommand FilterActiveVoucherCommand { get; set; }
        public FilterAllVoucherCommand FilterAllVoucherCommand { get; set; }
        public BindingVoucherSnackbar BindingVoucherSnackbar { get; set; }
        public RefreshData RefreshData { get; set; }

        public VoucherVM()
        {
            blockVouchers = new List<BlockVoucher>();
            ObservableBlockVouchers = new ObservableCollection<BlockVoucher>();

            vouchers = new List<Voucher>();
            ObservableVouchers = new ObservableCollection<Voucher>();

            GenerateBlockVoucher = new GenerateBlockVoucher(this);
            OpenAlertDialog = new OpenAlertDialog(this);
            DeleteBlockVoucherButton = new DeleteBlockVoucherButton(this);
            OpenVoucherCommand = new OpenVoucherCommand(this);
            FilterActiveVoucherCommand = new FilterActiveVoucherCommand(this);
            FilterAllVoucherCommand = new FilterAllVoucherCommand(this);

            BindingVoucherSnackbar = new BindingVoucherSnackbar(this);
            RefreshData = new RefreshData(this);
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
        public void LoadActiveVouchers()
        {
            vouchers = selectedBlockVoucher.vouchers;
            ObservableVouchers.Clear();
            for (int i = 0; i < vouchers.Count; ++i)
            {
                if (vouchers[i].Status == 0)
                {
                    ObservableVouchers.Add(vouchers[i]);
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
