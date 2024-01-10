using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class Consignment
{
    [Key]
    public int InputInfoId { get; set; }

    public string ProductId { get; set; } = null!;

    public int? Stock { get; set; }

    public DateTime? ManufacturingDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int? InputPrice { get; set; }

    public int? OutputPrice { get; set; }

    public double? Discount { get; set; }

    public int? InStock { get; set; }

    public virtual InputInfo InputInfo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
