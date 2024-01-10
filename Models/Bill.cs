using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class Bill
{
    [Key]
    public int Id { get; set; }

    public DateTime? BillDate { get; set; }

    public int? CustomerId { get; set; }

    public int? UserId { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual Customer? Customer { get; set; }

    public virtual User? User { get; set; }
}
