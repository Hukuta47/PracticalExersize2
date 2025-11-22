using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string FullName { get; set; } = null!;

    public string Birthday { get; set; } = null!;

    public int PassportId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string NumberPhone { get; set; } = null!;

    public virtual EmployeePassport Passport { get; set; } = null!;

    public virtual EmployeeRole Role { get; set; } = null!;
}
