using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model.Admin
{
    public class BlockVoucher
    {
        public int Id { get; set; }
        public string ReleaseName { get; set; }
        public int Type { get; set; }
        public int ParValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public List<Voucher> vouchers { get; set; }

        public BlockVoucher() 
        {
            vouchers = new List<Voucher>();
        }
    }
}
