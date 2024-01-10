using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class Voucher
{
    [Key]
    public string Code { get; set; } = null!;

    public int? Status { get; set; }

    public int? BlockId { get; set; }

    public virtual BlockVoucher? Block { get; set; }
}
