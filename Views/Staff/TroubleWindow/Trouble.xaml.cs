using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.Views.Staff.TroubleWindow
{
    /// <summary>
    /// Interaction logic for TroublePage.xaml
    /// </summary>
    public partial class Trouble : Page
    {
        public Trouble()
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
