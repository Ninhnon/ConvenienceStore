using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class InputInfo
{
    [Key]
    public int Id { get; set; }

    public DateTime? InputDate { get; set; }

    public int? UserId { get; set; }

    public int? SupplierId { get; set; }

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();

    public virtual Supplier? Supplier { get; set; }

    public virtual User? User { get; set; }
}
