using System.Windows;
using System.Windows.Controls;

namespace ConvenienceStore.Resources.UserControlCustom
{
    /// <summary>
    /// Interaction logic for TextBoxUC.xaml
    /// </summary>
    public partial class TextBoxUC : UserControl
    {
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(TextBoxUC));


        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(TextBoxUC));
        public TextBoxUC()
        {
            InitializeComponent();
        }
    }
}
