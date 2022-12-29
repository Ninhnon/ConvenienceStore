using ConvenienceStore.Model.Admin;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ConvenienceStore.Views.Admin.UserControls
{
    /// <summary>
    /// Interaction logic for BlockVoucherCard.xaml
    /// </summary>
    public partial class BlockVoucherCard : UserControl
    {
        public BlockVoucherCard()
        {
            InitializeComponent();
        }

        public BlockVoucher BlockVoucherItem
        {
            get { return (BlockVoucher)GetValue(BlockVoucherItemProperty); }
            set { SetValue(BlockVoucherItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlockVoucherItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlockVoucherItemProperty =
            DependencyProperty.Register("BlockVoucherItem", typeof(BlockVoucher), typeof(BlockVoucherCard), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BlockVoucherCard blockVoucherCardControl)
            {
                blockVoucherCardControl.DataContext = blockVoucherCardControl.BlockVoucherItem;
                blockVoucherCardControl.ActiveTextBlock.Text = $"{blockVoucherCardControl.BlockVoucherItem.vouchers.Count(p => p.Status == 0)} / {blockVoucherCardControl.BlockVoucherItem.vouchers.Count}";
            }
        }
    }
}
