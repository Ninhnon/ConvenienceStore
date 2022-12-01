using System;
using System.Collections.Generic;
using System.IO;
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

namespace ConvenienceStore.Staff
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
    }
}
