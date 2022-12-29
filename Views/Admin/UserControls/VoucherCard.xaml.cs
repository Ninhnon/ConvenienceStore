using ConvenienceStore.Model.Admin;
using System.Windows;
using System.Windows.Controls;

namespace ConvenienceStore.Views.Admin.UserControls
{
    /// <summary>
    /// Interaction logic for VoucherCard.xaml
    /// </summary>
    public partial class VoucherCard : UserControl
    {
        public VoucherCard()
        {
            InitializeComponent();
        }

        public Voucher VoucherItem
        {
            get { return (Voucher)GetValue(VoucherItemProperty); }
            set { SetValue(VoucherItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VoucherItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VoucherItemProperty =
            DependencyProperty.Register("VoucherItem", typeof(Voucher), typeof(VoucherCard), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VoucherCard voucherCardControl)
            {
                voucherCardControl.DataContext = voucherCardControl.VoucherItem;
            }
        }
    }
}
