using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetierGestion.Model
{
    public class ModePaiement
    {
        [Key]
        public int IdModePaiement { get; set; }
        [MaxLength(40)]
        public string LibelleModePaiement { get; set; }

        public virtual ICollection<Paiement> Paiements { get; set; }
    }
}
