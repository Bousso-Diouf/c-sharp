using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetierGestion.Model
{
    public class Locataire:Personne
    {
        [Required, MaxLength(20)]
        public string CNI { get; set; }

    }
}