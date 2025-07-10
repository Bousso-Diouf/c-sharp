using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Model
{
    public class Td_Erreur
    {

        [Key]
        public int Id { get; set; }

        public DateTime DateErreur { get; set; } = DateTime.Now;

        public string Name { get; set; }

        [MaxLength(2000)]
        public string DescriptionErreur { get; set; }

        [MaxLength(100)]

        public string TitreErreur { get; set; }




    }
}
