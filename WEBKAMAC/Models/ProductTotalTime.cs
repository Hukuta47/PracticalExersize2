namespace WEBKAMAC.Models
{
    public class ProductTotalTime
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal TotalManufacturingTime { get; set; }
        public int WorkshopCount { get; set; }
        public decimal TotalLaborHours { get; set; }
    }
}