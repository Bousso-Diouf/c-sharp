using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetierGestion.Model
{
    public class ContratLocation
    {
        [Key]
        public int IdContrat {  get; set; }
        [MaxLength(20)]
        public string Numero { get; set; }
        public DateTime? DateDebut { get; set;}
        public DateTime? DateFin { get; set;}
        public DateTime? DateCreation { get; set;}
        public float? Montant { get; set;}
        [MaxLength(20)]
        public string statut { get; set; }
        public int? IdAppartement { get; set; }

        [ForeignKey("IdAppartement")]
        public virtual Appartement Appartement { get; set; }

        public int? IdLocataire { get; set; }

        [ForeignKey("IdLocataire")]
        public virtual Locataire Locataire { get; set; }
        public int? IdPaiement { get; set; }

        [ForeignKey("IdPaiement")]
        public virtual Paiement Paiement { get; set; }

        public virtual ICollection<Paiement> Paiements { get; set; }
    }
}
