using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class Product
{
    [Key]
    public string Barcode { get; set; } = null!;

    public string? Title { get; set; }

    public byte[]? Image { get; set; }

    public string? Type { get; set; }

    public string? ProductionSite { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();
}
