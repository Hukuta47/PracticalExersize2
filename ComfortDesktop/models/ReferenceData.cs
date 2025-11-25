namespace ComfortDesktop.Models
{
    public class ProductType
    {
        public int ProductTypeID { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public decimal ProductTypeCoefficient { get; set; }
    }

    public class MaterialType
    {
        public int MaterialTypeID { get; set; }
        public string MaterialTypeName { get; set; } = string.Empty;
        public decimal RawMaterialLossPercent { get; set; }
    }

    public class WorkshopType
    {
        public int WorkshopTypeID { get; set; }
        public string WorkshopTypeName { get; set; } = string.Empty;
    }
}