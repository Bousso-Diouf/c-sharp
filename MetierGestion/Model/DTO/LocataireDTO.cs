using System.Runtime.Serialization;

namespace MetierGestion.Model.DTO
{
    [DataContract]
    public class LocataireDTO
    {
        [DataMember]
        public int IdPersonne { get; set; }

        [DataMember]
        public string NomPrenom { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string CNI { get; set; }
    }
}
