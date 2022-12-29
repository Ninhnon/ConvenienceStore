using ConvenienceStore.Model.Staff;
using System.Windows;
using System.Windows.Controls;

namespace ConvenienceStore.Views.Admin.UserControls
{
    /// <summary>
    /// Interaction logic for TroubleCard.xaml
    /// </summary>
    public partial class TroubleCard : UserControl
    {
        public TroubleCard()
        {
            InitializeComponent();
        }
        public Report ReportItem
        {
            get { return (Report)GetValue(ReportItemProperty); }
            set { SetValue(ReportItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReportItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReportItemProperty =
            DependencyProperty.Register("ReportItem", typeof(Report), typeof(TroubleCard), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TroubleCard ReportCardControl)
            {
                ReportCardControl.DataContext = ReportCardControl.ReportItem;
            }

        }
    }
}
