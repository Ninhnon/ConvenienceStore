using ConvenienceStore.ViewModel.Admin.AdminVM;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConvenienceStore.Views.Admin
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductView : Page
    {
        ProductVM VM;
        public ProductView()
        {
            InitializeComponent();
            VM = DataContext as ProductVM;
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VM.SetBlockSmallProductCorespondSearch();
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VM.SetBlockSmallProductCorespondSearch();
        }

        private void FilterTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM == null) return;

            var comboBox = sender as ComboBox;

            VM.ObservableSmallProducts.Clear();

            switch (comboBox.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < VM.smallProducts.Count; ++i)
                    {
                        if (VM.smallProducts[i].Title.ToLower().Contains(VM.SearchContent.ToLower()) || VM.smallProducts[i].Barcode.Contains(VM.SearchContent))
                        {
                            VM.ObservableSmallProducts.Add(VM.smallProducts[i]);
                        }
                    }
                    break;

                case 1:
                    for (int i = 0; i < VM.smallProducts.Count; ++i)
                    {
                        if (VM.smallProducts[i].Type == "Đồ ăn" &&
                            (VM.smallProducts[i].Title.ToLower().Contains(VM.SearchContent.ToLower()) || VM.smallProducts[i].Barcode.Contains(VM.SearchContent)))
                        {
                            VM.ObservableSmallProducts.Add(VM.smallProducts[i]);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < VM.smallProducts.Count; ++i)
                    {
                        if (VM.smallProducts[i].Type == "Thức uống" &&
                            (VM.smallProducts[i].Title.ToLower().Contains(VM.SearchContent.ToLower()) || VM.smallProducts[i].Barcode.Contains(VM.SearchContent)))
                        {
                            VM.ObservableSmallProducts.Add(VM.smallProducts[i]);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < VM.smallProducts.Count; ++i)
                    {
                        if (VM.smallProducts[i].Type == "Khác" &&
                            (VM.smallProducts[i].Title.ToLower().Contains(VM.SearchContent.ToLower()) || VM.smallProducts[i].Barcode.Contains(VM.SearchContent)))
                        {
                            VM.ObservableSmallProducts.Add(VM.smallProducts[i]);
                        }
                    }
                    break;
            }
        }
    }
}
