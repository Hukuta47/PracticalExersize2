using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class EmployeeRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Desription { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
