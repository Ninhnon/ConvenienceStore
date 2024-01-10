using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class BlockVoucher
{
    [Key]
    public int Id { get; set; }

    public string? ReleaseName { get; set; }

    public int? TypeVoucher { get; set; }

    public int? ParValue { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
