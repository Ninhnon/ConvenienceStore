using ConvenienceStore.ViewModel.Admin.AdminVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            this.HistoryDataGrid.Visibility = Visibility.Hidden;
        }

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
            this.SearchGrid.Visibility = Visibility.Visible;
            this.AccountsDataGrid.Visibility = Visibility.Visible;

        }

        private void HistoryClick(object sender, RoutedEventArgs e)
        {
            this.AccountsDataGrid.Visibility = Visibility.Hidden;
            this.SearchGrid.Visibility = Visibility.Hidden;
            this.HistoryDataGrid.Visibility = Visibility.Visible;
        }
    }
}
