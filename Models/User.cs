using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public string? UserRole { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public byte[]? Avatar { get; set; }

    public int? ManagerId { get; set; }

    public int? Salary { get; set; }

    public DateTime? SalaryDate { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<InputInfo> InputInfos { get; set; } = new List<InputInfo>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<SalaryBill> SalaryBills { get; set; } = new List<SalaryBill>();
}
