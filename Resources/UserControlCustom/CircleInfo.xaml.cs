using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConvenienceStore.Resources.UserControlCustom
{
    /// <summary>
    /// Interaction logic for CircleInfo.xaml
    /// </summary>
    public partial class CircleInfo : UserControl
    {
        public CircleInfo()
        {
            InitializeComponent();
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CircleInfo));


        public string Desc
        {
            get { return (string)GetValue(DescProperty); }
            set { SetValue(DescProperty, value); }
        }

        public static readonly DependencyProperty DescProperty = DependencyProperty.Register("Desc", typeof(string), typeof(CircleInfo));


        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(CircleInfo));
    }
}
