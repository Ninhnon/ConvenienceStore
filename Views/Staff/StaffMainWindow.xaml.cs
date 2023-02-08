using ConvenienceStore.Views.Login;
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
            MessageBoxCustom mb = new MessageBoxCustom("Thoát", "Bạn có chắc muốn thoát?", MessageType.Info, MessageButtons.YesNo);


            if (mb.ShowDialog() == true)
            {
                this.Close();
            }

        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxCustom mb = new MessageBoxCustom("Đăng xuất", "Bạn có chắc muốn đăng xuất?", MessageType.Info, MessageButtons.YesNo);


            if (mb.ShowDialog() == true)
            {
                LoginWindow log = new LoginWindow();

                this.Close();
                log.ShowDialog();
            }

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
