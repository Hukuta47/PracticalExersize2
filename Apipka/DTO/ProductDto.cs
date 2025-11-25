using System.Runtime.Serialization;

namespace Apipka.DTO
{
    [DataContract]
    public class ProductDto
    {
        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string ArticleNumber { get; set; }

        [DataMember]
        public decimal MinPartnerPrice { get; set; }

        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public string MaterialType { get; set; }

        [DataMember]
        public decimal? RawMaterialLossPercent { get; set; }

        // Добавляем ID для CRUD операций
        [DataMember]
        public int ProductTypeID { get; set; }

        [DataMember]
        public int MaterialTypeID { get; set; }
    }
}