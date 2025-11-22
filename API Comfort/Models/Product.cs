using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class Product
{
    public int Id { get; set; }

    public int ProductTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Article { get; set; } = null!;

    public double MinimumCostPartner { get; set; }

    public int ProductMaterialId { get; set; }

    public virtual ProductMaterial ProductMaterial { get; set; } = null!;

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual ICollection<ProductsWorkSpace> ProductsWorkSpaces { get; set; } = new List<ProductsWorkSpace>();
}
