namespace WEBKAMAC.Models
{
    public class Workshop
    {
        public int WorkshopID { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public string WorkshopType { get; set; } = string.Empty;
        public int WorkersCount { get; set; }
        public int WorkshopTypeID { get; set; }
    }
}