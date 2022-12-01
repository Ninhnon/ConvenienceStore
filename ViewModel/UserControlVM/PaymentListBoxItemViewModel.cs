using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvenienceStore.ViewModel.MainBase;

namespace ConvenienceStore.ViewModel.UserControlVM
{
    public class PaymentListBoxItemViewModel : BaseViewModel
    {
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private double? _DisplayPrice;
        public double? DisplayPrice { get => _DisplayPrice; set { _DisplayPrice = value; OnPropertyChanged(); } }

        private string _DisplayImage;
        public string DisplayImage { get => _DisplayImage; set { _DisplayImage = value; OnPropertyChanged(); } }

        public PaymentListBoxItemViewModel()
        {
            
        }
    }
}
