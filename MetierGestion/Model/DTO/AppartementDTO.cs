using System.Runtime.Serialization;

namespace MetierGestion.Model.DTO
{
    [DataContract]
    public class AppartementDTO
    {
        [DataMember]
        public int IdAppartement { get; set; }

        [DataMember]
        public string AdresseAppartement { get; set; }

        [DataMember]
        public float? Surface { get; set; }

        [DataMember]
        public float? Capacite { get; set; }

        [DataMember]
        public int? NombrePiece { get; set; }

        [DataMember]
        public bool? Disponible { get; set; }

        [DataMember]
        public int? IdProprietaire { get; set; }

        [DataMember]
        public string NomPrenomProprietaire { get; set; } // optionnel, utile pour affichage dans UI

        [DataMember]
        public int? IdTypeAppartement { get; set; }

        [DataMember]
        public string LibelleTypeAppartement { get; set; } // optionnel, utile pour affichage
    }
}
