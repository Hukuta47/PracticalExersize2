using System.Runtime.Serialization;

namespace Apipka.DTO
{
    [DataContract]
    public class ProductTypeDto
    {
        [DataMember]
        public int ProductTypeID { get; set; }

        [DataMember]
        public string ProductTypeName { get; set; }

        [DataMember]
        public decimal ProductTypeCoefficient { get; set; }
    }
}