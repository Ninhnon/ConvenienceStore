using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Login;
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

namespace ConvenienceStore.Views.Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
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

    }
}
