using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class EmployeePassport
{
    public int Id { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
