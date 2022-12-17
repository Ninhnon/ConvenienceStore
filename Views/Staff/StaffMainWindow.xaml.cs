using ConvenienceStore.Views.Login;
using System;
using System.Windows;

namespace ConvenienceStore.Views.Staff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StaffMainWindow : Window
    {
        public StaffMainWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();

            this.Close();
            login.ShowDialog();
          
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

        private void PagesNavigation_Navigated()
        {

        }
    }
}
