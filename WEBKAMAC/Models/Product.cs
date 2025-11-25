namespace WEBKAMAC.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ArticleNumber { get; set; } = string.Empty;
        public decimal MinPartnerPrice { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public decimal? RawMaterialLossPercent { get; set; }
        public int ProductTypeID { get; set; }
        public int MaterialTypeID { get; set; }
    }

    public class ProductCreateEditModel
    {
        public string ArticleNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int ProductTypeID { get; set; }
        public decimal MinPartnerPrice { get; set; }
        public int MaterialTypeID { get; set; }
    }
}