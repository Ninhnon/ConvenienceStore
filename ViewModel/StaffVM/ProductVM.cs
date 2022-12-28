using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Admin.ProductWindow;
using ConvenienceStore.Views.Login;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
