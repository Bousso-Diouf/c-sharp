using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetierGestion.Model
{
    public  class Td_Erreur
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateErreur {  get; set; }= DateTime.Now;
        [MaxLength(2000)]
        public string DescriptionErreur { get; set; }
        [MaxLength(200)]
        public string TitreErreur { get; set; }
    }
}
