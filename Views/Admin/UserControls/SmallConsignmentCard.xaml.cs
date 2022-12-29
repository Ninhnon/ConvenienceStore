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
    /// Interaction logic for SmallConsignmentCard.xaml
    /// </summary>
    public partial class SmallConsignmentCard : UserControl
    {
        public SmallConsignmentCard()
        {
            InitializeComponent();
        }

        public SmallConsignment MyProperty
        {
            get { return (SmallConsignment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(SmallConsignment), typeof(SmallConsignmentCard), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SmallConsignmentCard inputInfoCardControl)
            {
                inputInfoCardControl.DataContext = inputInfoCardControl.MyProperty;

                if (inputInfoCardControl.MyProperty.ExperyDate < DateTime.Now)
                {
                    inputInfoCardControl.ExperyDateTextBlock.Foreground = Brushes.Red;
                }
            }
        }
    }
}
