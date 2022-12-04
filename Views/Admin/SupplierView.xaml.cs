using ConvenienceStore.ViewModel.Admin.AdminVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConvenienceStore.Views.Admin
{
    /// <summary>
    /// Interaction logic for SupplierView.xaml
    /// </summary>
    public partial class SupplierView : Page
    {
        public SupplierView()
        {
            InitializeComponent();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var searchTextBox = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                var VM = searchTextBox.DataContext as SupplierVM;
                VM.SetSupplierCorespondSearch();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = (Border)sender;
            var VM = button.DataContext as SupplierVM;
            VM.SetSupplierCorespondSearch();
        }
    }
}
