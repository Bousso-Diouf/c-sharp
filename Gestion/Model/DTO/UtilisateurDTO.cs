using System.Runtime.Serialization;

namespace Gestion.Model.DTO
{
    [DataContract]
    public class UtilisateurDTO
    {
        [DataMember]
        public int IdPersonne { get; set; }

        [DataMember]
        public string NomPrenom { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string Identifiant { get; set; }

        [DataMember]
        public string MotDePasse { get; set; }

        [DataMember]
        public string Statut { get; set; }
    }
}
