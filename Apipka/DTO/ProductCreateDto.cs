using System.Runtime.Serialization;

namespace Apipka.DTO
{
    [DataContract]
    public class ProductCreateDto
    {
        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string ArticleNumber { get; set; }

        [DataMember]
        public decimal MinPartnerPrice { get; set; }

        [DataMember]
        public int ProductTypeID { get; set; }

        [DataMember]
        public int MaterialTypeID { get; set; }
    }
}