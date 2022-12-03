using ConvenienceStore.Model.Admin;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConvenienceStore.Views.Admin.UserControls
{
    /// <summary>
    /// Interaction logic for InputInfoCard.xaml
    /// </summary>
    public partial class InputInfoCard : UserControl
    {
        public InputInfoCard()
        {
            InitializeComponent();
        }
        public InputInfo InputInfoItem
        {
            get { return (InputInfo)GetValue(InputInfoItemProperty); }
            set { SetValue(InputInfoItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputInfoItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputInfoItemProperty =
            DependencyProperty.Register("InputInfoItem", typeof(InputInfo), typeof(InputInfoCard), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is InputInfoCard inputInfoCardControl)
            {
                inputInfoCardControl.DataContext = inputInfoCardControl.InputInfoItem;
            }

        }
    }
}
