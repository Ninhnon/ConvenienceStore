using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class SalaryBill
{
    [Key]
    public int Id { get; set; }

    public DateTime? SalaryBillDate { get; set; }

    public int? UserId { get; set; }

    public int? TotalMoney { get; set; }

    public virtual User? User { get; set; }
}
