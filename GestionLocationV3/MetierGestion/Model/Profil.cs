using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetierGestion.Model
{
    public class Profil
    {
        [Key]
        public int IdProfil {  get; set; }

        [Required, MaxLength(50)]
        public string LibelleProfil { get; set; }
    }
}
