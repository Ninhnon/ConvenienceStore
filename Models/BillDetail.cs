using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class BillDetail
{
    [Key]
    public int BillId { get; set; }

    public string ProductId { get; set; } = null!;

    public int? Quantity { get; set; }

    public int? TotalPrice { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
