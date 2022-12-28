using ConvenienceStore.ViewModel.Admin.AdminVM;
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

namespace ConvenienceStore.Views.Admin
{
    /// <summary>
    /// Interaction logic for InputInfoView.xaml
    /// </summary>
    public partial class InputInfoView : Page
    {
        InputInfoVM VM;
        public InputInfoView()
        {
            InitializeComponent();
            VM = DataContext as InputInfoVM;
        }
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VM.SetProductsCorrespondSearch();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var VM = comboBox.DataContext as InputInfoVM;
            if (comboBox.SelectedIndex == 0)
            {
                var ascInputInfos = VM.ObservableInputInfos.OrderBy(e => e.InputDate).ToList();
                VM.ObservableInputInfos.Clear();
                for (int i = 0; i < ascInputInfos.Count; ++i)
                {
                    VM.ObservableInputInfos.Add(ascInputInfos[i]);
                }
            }
            else
            {
                var descInputInfos = VM.ObservableInputInfos.OrderByDescending(e => e.InputDate).ToList();
                VM.ObservableInputInfos.Clear();
                for (int i = 0; i < descInputInfos.Count; ++i)
                {
                    VM.ObservableInputInfos.Add(descInputInfos[i]);
                }
            }
        }
    }
}
