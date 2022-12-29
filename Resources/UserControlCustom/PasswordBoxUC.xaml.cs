using System.Windows;
using System.Windows.Controls;

namespace ConvenienceStore.Resources.UserControlCustom
{
    /// <summary>
    /// Interaction logic for PasswordBoxUC.xaml
    /// </summary>
    public partial class PasswordBoxUC : UserControl
    {
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(PasswordBoxUC));


        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(PasswordBoxUC));
        public PasswordBoxUC()
        {
            InitializeComponent();
        }

        private void pwdChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
                textBoxmain.Visibility = Visibility.Visible;
            else textBoxmain.Visibility = Visibility.Hidden;
        }
    }
}
