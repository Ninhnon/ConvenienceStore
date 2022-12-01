using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model.Lam
{
    public class Product
    {
        public int InputInfoId { get; set; }
        public string Barcode { get; set; }
        public string Title { get; set; }
        public string ProductionSite { get; set; }
        public byte[] Image { get; set; }
        public int Cost { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Discount { get; set; }
    }
}
