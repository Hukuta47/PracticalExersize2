using System.Runtime.Serialization;

namespace Apipka.DTO
{
    [DataContract]
    public class WorkshopTypeDto
    {
        [DataMember]
        public int WorkshopTypeID { get; set; }

        [DataMember]
        public string WorkshopTypeName { get; set; }
    }
}