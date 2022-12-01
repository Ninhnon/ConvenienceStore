using System;
using System.Windows;

namespace ConvenienceStore.Views.Staff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StaffMainWindow : Window
    {
        private Uri paymentPage = new Uri("Views/Staff/PaymentWindow.xaml", UriKind.RelativeOrAbsolute);
        private Uri historyPage = new Uri("Views/Staff/HistoryWindow.xaml", UriKind.RelativeOrAbsolute);
        private Uri profilePage = new Uri("Views/Staff/ProfileWindow.xaml", UriKind.RelativeOrAbsolute);

        public StaffMainWindow()
        {
            InitializeComponent();
            PagesNavigation.Navigate(new System.Uri("Views/Staff/PaymentWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void rdPayment_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new Uri("Views/Staff/PaymentWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdHistory_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new Uri("Views/Staff/HistoryWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdProfile_Click(object sender, RoutedEventArgs e)
        {
            // PagesNavigation.Navigate(new HomePage());
            PagesNavigation.Navigate(new System.Uri("Views/Staff/ProfileWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdProduct_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/Staff/ProductWindow/ProductPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdVoucher_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/Staff/VoucherWindow/VoucherPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdReport_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/Staff/TroubleWindow/TroublePage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
