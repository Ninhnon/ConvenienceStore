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
using System.Windows.Shapes;

namespace ConvenienceStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //PagesNavigation.Navigate(new Uri("Views/InputInfoView.xaml", UriKind.RelativeOrAbsolute));
            PagesNavigation.Navigate(new System.Uri("Views/HomeView.xaml", UriKind.RelativeOrAbsolute));
        }
        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ok(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
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

        private void rdHome_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/HomeView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdInputInfo_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new Uri("Views/InputInfoView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdProfile_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/ProfileView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdSupplier_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/SupplierView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdVoucher_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/Staff/VoucherWindow/VoucherPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void rdReport_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Views/ReportView.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
