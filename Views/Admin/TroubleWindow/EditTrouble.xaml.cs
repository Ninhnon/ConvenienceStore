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

namespace ConvenienceStore.Views.Admin.TroubleWindow
{
    /// <summary>
    /// Interaction logic for EditTrouble.xaml
    /// </summary>
    public partial class EditTrouble : Window
    {
        public EditTrouble()
        {
            InitializeComponent();
        }
        private void ImagePreview_MouseEnter(object sender, MouseEventArgs e)
        {
            MarkUploadImage.Visibility = Visibility.Visible;
        }

        private void MarkUploadImage_MouseLeave(object sender, MouseEventArgs e)
        {
            MarkUploadImage.Visibility = Visibility.Hidden;
        }

        private void cbxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxStatus.SelectedIndex==2)
            {
                if (_start1 != null)
                {
                    _start1.IsEnabled = true;
                    _start1.Visibility = Visibility.Visible;
                    StartDate.SelectedDate = DateTime.Now;
                }
                if (_finish1 != null)
                {
                    _finish1.IsEnabled = true;
                    _finish1.Visibility = Visibility.Visible;
                    FinishDate.SelectedDate= DateTime.Now;
                }
            }
            else if (cbxStatus.SelectedIndex == 1)
            {
                if (_start1 != null)
                {
                    _start1.IsEnabled = true;
                    _start1.Visibility = Visibility.Visible;
                    StartDate.SelectedDate= DateTime.Now;
                }
                if (_finish1 != null)
                {
                    _finish1.IsEnabled = false;
                    _finish1.Visibility = Visibility.Collapsed;
                }
            }
            else if (cbxStatus.SelectedIndex == 0)
            {
                if (_start != null && _start1 != null)
                {
                    _start1.IsEnabled = false;
                    _start1.Visibility = Visibility.Collapsed;
                }
                if (_finish != null && _finish1 != null)
                {
                    _finish1.IsEnabled = false;
                    _finish1.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void cbxStatus_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbxStatus.SelectedIndex == 2)
            {
                if (_start1 != null)
                {
                    _start1.IsEnabled = true;
                    _start1.Visibility = Visibility.Visible;
                }
                if (_finish1 != null)
                {
                    _finish1.IsEnabled = true;
                    _finish1.Visibility = Visibility.Visible;
                }
            }
            else if (cbxStatus.SelectedIndex == 1)
            {
                if (_start1 != null)
                {
                    _start1.IsEnabled = true;
                    _start1.Visibility = Visibility.Visible;
                }
                if (_finish1 != null)
                {
                    _finish1.IsEnabled = false;
                    _finish1.Visibility = Visibility.Collapsed;
                }
            }
            else if (cbxStatus.SelectedIndex == 0)
            {
                if (_start != null && _start1 != null)
                {
                    _start1.IsEnabled = false;
                    _start1.Visibility = Visibility.Collapsed;
                }
                if (_finish != null && _finish1 != null)
                {
                    _finish1.IsEnabled = false;
                    _finish1.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
