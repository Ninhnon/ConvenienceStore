using System.Windows;
using System.Windows.Controls;

namespace ConvenienceStore.Resources.UserControlCustom
{
    /// <summary>
    /// Interaction logic for TextBox1.xaml
    /// </summary>
    public partial class TextBox1 : UserControl
    {
        public TextBox1()
        {
            InitializeComponent();
        }
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(TextBox1));
    }
}
