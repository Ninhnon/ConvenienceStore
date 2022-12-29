using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model.Admin
{
    public class SmallConsignment
    {
        public int Id { get; set; }

        public int InStock { get; set; }

        public DateTime ExperyDate { get; set; }

        public double Discount { get; set; }
    }
}
