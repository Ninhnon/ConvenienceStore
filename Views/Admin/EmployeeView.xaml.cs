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
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Page
    {
        public EmployeeView()
        {
            InitializeComponent();
            this.HistoryDataGrid.Visibility = Visibility.Hidden;        }

        private void enter(object sender, KeyEventArgs e)
        {
            var searchTextBox = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                var VM = searchTextBox.DataContext as EmployeeViewModel;
               
                VM.Search();
            }
        }

        private void EmployeeClick(object sender, RoutedEventArgs e)
        {
            this.HistoryDataGrid.Visibility = Visibility.Hidden;
            this.AccountsDataGrid.Visibility = Visibility.Visible;
          
        }

        private void HistoryClick(object sender, RoutedEventArgs e)
        {
            this.AccountsDataGrid.Visibility = Visibility.Hidden;
            this.HistoryDataGrid.Visibility = Visibility.Visible;
        }
    }
}
