using System;
using System.Collections.Generic;

namespace LogisticCompany.models;

public partial class Employee
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CompanyId { get; set; }

    public virtual Company? Company { get; set; }
}
