namespace WEBKAMAC.Models
{
    public class ProductionSummary
    {
        public string ProductType { get; set; } = string.Empty;
        public string Workshop { get; set; } = string.Empty;
        public decimal TotalTime { get; set; }
        public decimal AverageTime { get; set; }
        public int ProductCount { get; set; }
    }
}