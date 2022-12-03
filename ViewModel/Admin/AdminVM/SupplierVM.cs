using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin.Command.SupplierCommand.AddNewSupplierCommand;
using ConvenienceStore.ViewModel.Admin.Command.SupplierCommand.SupplierCard;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class SupplierVM : INotifyPropertyChanged
    {
        public List<Supplier> suppliers { get; set; }

        public ObservableCollection<Supplier> ObservableSupplier { get; set; }

        private Supplier selectedSupplier;

        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                selectedSupplier = value;
                OnPropertyChanged("SelectedSupplier");
            }
        }

        private string searchContent;

        public string SearchContent
        {
            get { return searchContent; }
            set
            {
                searchContent = value;
                OnPropertyChanged("SearchContent");

            }
        }

        public AddSupplierButtonCommand AddSupplierButtonCommand { get; set; }
        public DeleteSupplierButton DeleteSupplierButton { get; set; }
        public SupplierVM()
        {
            suppliers = DatabaseHelper.FetchingSupplier();
            ObservableSupplier = new ObservableCollection<Supplier>();
            for (int i = 0; i < suppliers.Count; i++)
            {
                ObservableSupplier.Add(suppliers[i]);
            }

            AddSupplierButtonCommand = new AddSupplierButtonCommand(this);
            DeleteSupplierButton = new DeleteSupplierButton(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
