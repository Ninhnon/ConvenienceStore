using System.Windows;
using System.Windows.Input;

namespace ConvenienceStore.Views.Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxCustom mb = new MessageBoxCustom("Thoát", "Bạn có chắc muốn thoát?", MessageType.Info, MessageButtons.YesNo);


            if (mb.ShowDialog() == true)
            {
                Application.Current.Shutdown();
            }

        }
    }
}
