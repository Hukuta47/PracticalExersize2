using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class WorkspaceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>();
}
