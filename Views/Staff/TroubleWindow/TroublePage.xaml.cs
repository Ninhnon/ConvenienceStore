using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.Views.Staff.TroubleWindow
{
    /// <summary>
    /// Interaction logic for TroubleView.xaml
    /// </summary>
    public partial class TroublePage : Page
    {
        public TroublePage()
        {
            InitializeComponent();
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
