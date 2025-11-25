namespace ComfortDesktop.Models
{
    public class ProductionSummary
    {
        public string ProductType { get; set; } = string.Empty;
        public string Workshop { get; set; } = string.Empty;
        public decimal TotalTime { get; set; }
        public decimal AverageTime { get; set; }
        public int ProductCount { get; set; }
    }

    public class ProductTotalTime
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal TotalManufacturingTime { get; set; }
        public int WorkshopCount { get; set; }
        public decimal TotalLaborHours { get; set; }
    }
}