using System.Runtime.Serialization;

namespace Apipka.DTO
{
    [DataContract]
    public class WorkshopDto
    {
        [DataMember]
        public int WorkshopID { get; set; }

        [DataMember]
        public string WorkshopName { get; set; }

        [DataMember]
        public string WorkshopType { get; set; }

        [DataMember]
        public int WorkersCount { get; set; }

        [DataMember]
        public int WorkshopTypeID { get; set; }
    }
}