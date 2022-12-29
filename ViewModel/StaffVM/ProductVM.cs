using ConvenienceStore.Model.Admin;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ConvenienceStore.ViewModel.StaffVM
{
    public class ProductVM : INotifyPropertyChanged
    {
        public List<Supplier> suppliers { get; set; }

        public ObservableCollection<Supplier> ObservableSupplier { get; set; }

        private string searchContent;

        public string SearchContent
        {
            get { return searchContent; }
            set
            {
                searchContent = value;
                OnPropertyChanged("SearchContent");

                if (searchContent == "")
                {
                    ObservableSupplier.Clear();
                    for (int i = 0; i < suppliers.Count; i++)
                    {
                        ObservableSupplier.Add(suppliers[i]);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
