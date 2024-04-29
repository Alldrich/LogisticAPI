using System;
using System.Collections.Generic;

namespace LogisticCompany.models;

public partial class Company
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateOnly CreationDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    
}
