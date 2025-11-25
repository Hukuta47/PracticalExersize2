namespace WEBKAMAC.Models
{
    public class ManufacturingData
    {
        public int ProductManufacturingID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string WorkshopName { get; set; } = string.Empty;
        public string WorkshopType { get; set; } = string.Empty;
        public decimal ManufacturingTimeHours { get; set; }
        public int WorkersCount { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public decimal MinPartnerPrice { get; set; }
    }
}