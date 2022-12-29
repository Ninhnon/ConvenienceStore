using System.Windows;
using System.Windows.Input;

namespace ConvenienceStore.Views.Staff.TroubleWindow
{
    /// <summary>
    /// Interaction logic for AddTrouble.xaml
    /// </summary>
    public partial class AddTrouble : Window
    {
        public AddTrouble()
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
    }
}
