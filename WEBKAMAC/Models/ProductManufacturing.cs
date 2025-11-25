namespace WEBKAMAC.Models
{
    public class ProductManufacturing
    {
        public int ProductManufacturingID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int WorkshopID { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public decimal ManufacturingTimeHours { get; set; }
        public string WorkshopType { get; set; } = string.Empty;
        public int WorkersCount { get; set; }
    }
}