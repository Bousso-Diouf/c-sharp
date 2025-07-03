using System.Runtime.Serialization;

namespace MetierGestion.Model.DTO
{
    [DataContract]
    public class ProprietaireDTO
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
        public string Ninea { get; set; }

        [DataMember]
        public string Rccm { get; set; }
    }
}
