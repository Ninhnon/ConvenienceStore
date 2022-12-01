using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore.Model.Lam
{
    public class InputInfo
    {
        public int Id { get; set; }
        public DateTime InputDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set;}
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] Avatar { get; set; }
        public int SupplerId { get; set; }
        public string SupplierName { get; set; }
        public List<Product> products { get; set; }

        public InputInfo()
        {
            products = new List<Product>();
        }
    }
}
