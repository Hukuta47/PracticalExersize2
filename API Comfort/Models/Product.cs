using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API_Comfort.Models;

public partial class Product
{
    public int Id { get; set; }

    public int ProductTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Article { get; set; } = null!;

    public double MinimumCostPartner { get; set; }

    public int ProductMaterialId { get; set; }
    [JsonIgnore]
    public virtual ProductMaterial ProductMaterial { get; set; } = null!;
    [JsonIgnore]
    public virtual ProductType ProductType { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<ProductsWorkSpace> ProductsWorkSpaces { get; set; } = new List<ProductsWorkSpace>();
}
