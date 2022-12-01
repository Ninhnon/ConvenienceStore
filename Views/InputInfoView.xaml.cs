using ConvenienceStore.ViewModel.Lam;
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

namespace ConvenienceStore.Views
{
    /// <summary>
    /// Interaction logic for InputInfoView.xaml
    /// </summary>
    public partial class InputInfoView : Page
    {
        public InputInfoView()
        {
            InitializeComponent();
        }
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var searchTextBox = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                var VM = searchTextBox.DataContext as InputInfoVM;
                VM.SetProductsCorrespondSearch();
            }
        }

    }
}
