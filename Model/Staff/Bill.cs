namespace ConvenienceStore.Model.Staff
{
    using System;
    using System.Collections.Generic;
    
    public class Bill
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<decimal> Price { get; set; }
    }
}
