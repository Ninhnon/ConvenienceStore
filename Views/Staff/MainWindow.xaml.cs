using System;
using System.Windows;

namespace ConvenienceStore.Views.Staff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Uri paymentPage = new Uri("Views/Staff/PaymentWindow.xaml", UriKind.RelativeOrAbsolute);
        private Uri historyPage = new Uri("Views/Staff/HistoryWindow.xaml", UriKind.RelativeOrAbsolute);
        private Uri profilePage = new Uri("Views/Staff/ProfileWindow.xaml", UriKind.RelativeOrAbsolute);

        public MainWindow()
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
            PagesNavigation.Navigate(new System.Uri("Views/Staff/PaymentWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdHistory_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/Staff/HistoryWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdProfile_Click(object sender, RoutedEventArgs e)
        {
            // PagesNavigation.Navigate(new HomePage());
            PagesNavigation.Navigate(new System.Uri("Views/Staff/ProfileWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdVoucher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdReport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
