using ConvenienceStore.ViewModel.Admin.AdminVM;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

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
