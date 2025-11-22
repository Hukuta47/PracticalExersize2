using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class Workspace
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeWorkspaceId { get; set; }

    public int NumberPeopleForProduciton { get; set; }

    public virtual ICollection<ProductsWorkSpace> ProductsWorkSpaces { get; set; } = new List<ProductsWorkSpace>();

    public virtual WorkspaceType TypeWorkspace { get; set; } = null!;
}
