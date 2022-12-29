using ConvenienceStore.ViewModel.Admin.AdminVM;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.Views.Admin
{
    /// <summary>
    /// Interaction logic for VoucherView.xaml
    /// </summary>
    public partial class VoucherView : Page
    {
        VoucherVM VM;
        public VoucherView()
        {
            InitializeComponent();

            VM = DataContext as VoucherVM;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VM.SetBlockVoucherCorespondSearch();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VM.SetBlockVoucherCorespondSearch();
            }
        }
    }
}
