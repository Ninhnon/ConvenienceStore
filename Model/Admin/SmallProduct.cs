using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model.Admin
{
    public class SmallProduct
    {
        public string Barcode { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Type { get; set; }
        public string ProductionSite { get; set; }
        public int Stock { get; set; }
        public ObservableCollection<SmallConsignment> ObservableSmallConsignments { get; set; }

        public SmallProduct()
        {
            ObservableSmallConsignments = new ObservableCollection<SmallConsignment>();
        }

    }
}
