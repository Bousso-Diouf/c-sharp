using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Model
{
    public class Produit
    {
        public int IdProduit { get; set; }
        public string CodeProduit { get; set; }
        public string DesignationProduit { get; set; }
        public Nullable<int> QteMin { get; set; }
        public Nullable<decimal> PU { get; set; }
    }
}
