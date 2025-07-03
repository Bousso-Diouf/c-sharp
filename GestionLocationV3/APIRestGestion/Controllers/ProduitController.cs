using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIRestGestion.Models;


namespace APIRestGestion.Controllers
{
    public class ProduitController : ApiController
    {
        bdLivraisonEntities db = new bdLivraisonEntities();

        public List<Produit> GetAllProduits()
        {
            return db.Produit.ToList();
        }

        public bool AddProduit(Produit produit)
        {
            try
            {
                db.Produit.Add(produit);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
