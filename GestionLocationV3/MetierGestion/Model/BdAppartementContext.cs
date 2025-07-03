using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace MetierGestion.Model
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BdAppartementContext:DbContext
    {
        public BdAppartementContext() :base("bdAppartementContext")
        { }

        public DbSet<Profil> profils { get; set; }
        public DbSet<Appartement> appartements { get; set; }
        public DbSet<Proprietaire> proprietaires { get; set; }
        public DbSet<Locataire> locataires { get; set; }
        public DbSet<Personne> personnes { get; set; }
        public DbSet<TypeAppartement> typeAppartements { get; set; }
        public DbSet<ContratLocation> contratLocations { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<ModePaiement> modePaiements { get; set; }
        public DbSet<Gestionnaire> Gestionnaires { get; set; }
        public DbSet<Admin> admin { get; set; }

        public DbSet<Td_Erreur> td_Erreurs { get; set; }
    }
}
