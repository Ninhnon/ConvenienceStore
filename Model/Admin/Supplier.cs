using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model.Admin
{
    public class Supplier
    {
        public int Id { get; set; }
        public int Number { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
