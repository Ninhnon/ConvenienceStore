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
using System.Windows.Shapes;
using ConvenienceStore.ViewModel.StaffVM;

namespace ConvenienceStore.Views.Staff
{
    /// <summary>
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        public Receipt()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void Print_Bill(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    //Lưu giá trị cũ
                    var a = billDetailCard.Height;
                    var b = receiptPage.Height;

                    //Ẩn nút để in hóa đơn
                    PrintBtn.Visibility = Visibility.Collapsed;
                    OkBtn.Visibility = Visibility.Collapsed;
                    CancelBtn.Visibility = Visibility.Collapsed;

                    if (billDetailListView.ActualHeight > billDetailCard.Height)
                        receiptPage.Height = receiptPage.Height + billDetailListView.ActualHeight - billDetailCard.Height;

                    billDetailCard.Height = billDetailListView.ActualHeight;

                    billDetailScrollView.ScrollToHome();
                    billDetailScrollView.UpdateDefaultStyle();

                    printDialog.PrintVisual(receiptPage, "Receipt");

                    PrintBtn.Visibility = Visibility.Visible;
                    OkBtn.Visibility = Visibility.Visible;
                    CancelBtn.Visibility = Visibility.Visible;

                    //Gán trở lại
                    billDetailCard.Height = a;
                    receiptPage.Height = b;
                }
            }
            catch
            {
                
            }
        }
    }
}
