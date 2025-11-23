using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API_Comfort.Models;

public partial class ProductsWorkSpace
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WorkspaceId { get; set; }

    public double ProductionTimePerHour { get; set; }
    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
    [JsonIgnore]
    public virtual Workspace Workspace { get; set; } = null!;
}
