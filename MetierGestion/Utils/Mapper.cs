using MetierGestion.Model;
using MetierGestion.Model.DTO;
using System.Collections.Generic;
using System.Linq;

namespace MetierGestion.Utils
{
    public static class Mapper
    {
        // ------ PROPRIETAIRE ------

        public static ProprietaireDTO ToDTO(this Proprietaire proprietaire)
        {
            if (proprietaire == null) return null;

            return new ProprietaireDTO
            {
                IdPersonne = proprietaire.IdPersonne,
                NomPrenom = proprietaire.NomPrenom,
                Telephone = proprietaire.Telephone,
                Email = proprietaire.Email,
                Ninea = proprietaire.Ninea,
                Rccm = proprietaire.Rccm
            };
        }

        public static List<ProprietaireDTO> ToDTO(this List<Proprietaire> proprietaires)
        {
            return proprietaires.Select(p => p.ToDTO()).ToList();
        }

        // ------ APPARTEMENT ------

        public static AppartementDTO ToDTO(this Appartement appartement)
        {
            if (appartement == null) return null;

            return new AppartementDTO
            {
                IdAppartement = appartement.IdAppartement,
                AdresseAppartement = appartement.AdresseAppartement,
                Surface = appartement.Surface,
                Capacite = appartement.Capacite,
                NombrePiece = appartement.NombrePiece,
                Disponible = (bool)appartement.Disponible,
                IdProprietaire = appartement.IdProprietaire,
                NomPrenomProprietaire = appartement.Proprietaire?.NomPrenom, // pour affichage
                IdTypeAppartement = appartement.IdTypeAppartement,
            };
        }

        public static List<AppartementDTO> ToDTO(this List<Appartement> appartements)
        {
            return appartements.Select(a => a.ToDTO()).ToList();
        }
    }
}
