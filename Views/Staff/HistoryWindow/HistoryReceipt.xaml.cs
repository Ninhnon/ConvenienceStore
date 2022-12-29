using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.Views.Staff
{
    /// <summary>
    /// Interaction logic for HistoryReceipt.xaml
    /// </summary>
    public partial class HistoryReceipt : Window
    {
        public HistoryReceipt()
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
