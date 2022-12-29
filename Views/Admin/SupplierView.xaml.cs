using ConvenienceStore.ViewModel.Admin.AdminVM;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.Views.Admin
{
    /// <summary>
    /// Interaction logic for SupplierView.xaml
    /// </summary>
    public partial class SupplierView : Page
    {
        SupplierVM VM;
        public SupplierView()
        {
            InitializeComponent();
            VM = DataContext as SupplierVM;
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VM.SetSupplierCorespondSearch();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VM.SetSupplierCorespondSearch();
        }
    }
}
