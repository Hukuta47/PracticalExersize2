using System.Runtime.Serialization;

namespace Apipka.DTO
{
    [DataContract]
    public class MaterialTypeDto
    {
        [DataMember]
        public int MaterialTypeID { get; set; }

        [DataMember]
        public string MaterialTypeName { get; set; }

        [DataMember]
        public decimal RawMaterialLossPercent { get; set; }
    }
}