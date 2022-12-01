using ConvenienceStore.Model.Lam;
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

namespace ConvenienceStore.Views.Lam.UserControls
{
    /// <summary>
    /// Interaction logic for ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        public ProductCard()
        {
            InitializeComponent();
        }
        public Product ProductItem
        {
            get { return (Product)GetValue(ProductItemProperty); }
            set { SetValue(ProductItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductItemProperty =
            DependencyProperty.Register("ProductItem", typeof(Product), typeof(ProductCard), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductCard productCardControl)
            {
                productCardControl.DataContext = productCardControl.ProductItem;
            }
        }
    }
}
