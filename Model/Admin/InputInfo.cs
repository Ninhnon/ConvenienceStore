using System;
using System.Collections.Generic;

namespace ConvenienceStore.Model.Admin
{
    public class InputInfo
    {
        public int Id { get; set; }
        public DateTime InputDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] Avatar { get; set; }
        public string SupplierName { get; set; }
        public List<Product> products { get; set; }

        public InputInfo()
        {
            products = new List<Product>();
        }
    }
}
