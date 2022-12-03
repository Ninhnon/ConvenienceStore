using ConvenienceStore.Model.Staff;
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

namespace ConvenienceStore.Views.Staff.VoucherWindow
{
    /// <summary>
    /// Interaction logic for VoucherPage.xaml
    /// </summary>
    public partial class VoucherPage : Page
    {
        public VoucherPage()
        {
            InitializeComponent();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(VoucherListView.ItemsSource);
            view.Filter = Filter;
            result.Content = VoucherListView.Items.Count;
            CollectionViewSource.GetDefaultView(VoucherListView.ItemsSource).Refresh();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(FilterBox.Text))
                return true;
            else
                return ((Vouchers)item).ReleaseId.Contains(FilterBox.Text, StringComparison.OrdinalIgnoreCase);
        }
    }
}
