using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class ProductMaterial
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double PercentageMaterialLosses { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
