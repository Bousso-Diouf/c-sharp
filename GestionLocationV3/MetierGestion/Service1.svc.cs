using MetierGestion.Model;
using MetierGestion.Model.DTO;
using MetierGestion.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;


namespace MetierGestion
{
    public class Service1 : IService1
    {
        BdAppartementContext db = new BdAppartementContext();
        Helper helper = new Helper();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
                throw new ArgumentNullException("composite");

            if (composite.BoolValue)
                composite.StringValue += "Suffix";

            return composite;
        }

        /// <summary>
        /// Ajouter un appartement via DTO
        /// </summary>
        public bool AddAppartement(AppartementDTO dto)
        {
            try
            {
                var appartement = new Appartement
                {
                    AdresseAppartement = dto.AdresseAppartement,
                    Surface = dto.Surface,
                    Capacite = dto.Capacite,
                    NombrePiece = dto.NombrePiece,
                    Disponible = dto.Disponible,
                    IdProprietaire = dto.IdProprietaire,
                    IdTypeAppartement = dto.IdTypeAppartement
                };

                if (dto.IdProprietaire == null)
                {
                    helper.WriteDataError("AddAppartement", "Propriétaire non sélectionné.");
                    return false;
                }


                db.appartements.Add(appartement);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                helper.WriteDataError("AddAppartement", ex.InnerException?.Message ?? ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                helper.WriteDataError("Service1-AddAppartement", ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Mise à jour via DTO
        /// </summary>
        public bool UpdateAppartement(AppartementDTO dto)
        {
            try
            {
                var appartement = db.appartements.Find(dto.IdAppartement);
                if (appartement == null) return false;

                // Mise à jour manuelle
                appartement.AdresseAppartement = dto.AdresseAppartement;
                appartement.Surface = dto.Surface;
                appartement.Capacite = dto.Capacite;
                appartement.NombrePiece = dto.NombrePiece;
                appartement.Disponible = dto.Disponible;
                appartement.IdProprietaire = dto.IdProprietaire;
                appartement.IdTypeAppartement = dto.IdTypeAppartement;

                db.Entry(appartement).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                helper.WriteDataError("Service1-UpdateAppartement", ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Suppression d’un appartement par ID
        /// </summary>
        public bool DeleteAppartement(int idAppartement)
        {
            try
            {
                var appartement = db.appartements.Find(idAppartement);
                if (appartement == null) return false;

                db.Entry(appartement).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                helper.WriteDataError("Service1-DeleteAppartement", ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Liste des appartements avec filtres et conversion DTO
        /// </summary>
        public List<AppartementDTO> GetListeAppartement(string adresse, float? capacite, bool? disponible)
        {
            var liste = db.appartements.Include(a => a.Proprietaire).ToList();

            if (!string.IsNullOrEmpty(adresse))
                liste = liste.Where(a => a.AdresseAppartement.ToLower().Contains(adresse.ToLower())).ToList();

            if (capacite.HasValue)
                liste = liste.Where(a => a.Capacite == capacite).ToList();

            if (disponible.HasValue)
                liste = liste.Where(a => a.Disponible == disponible).ToList();

            return liste.ToDTO();
        }

        /// <summary>
        /// Récupération d’un appartement par ID avec conversion DTO
        /// </summary>
        public AppartementDTO GetAppartementById(int? id)
        {
            var appartement = db.appartements.Include(a => a.Proprietaire)
                                             .FirstOrDefault(a => a.IdAppartement == id);
            return appartement?.ToDTO();
        }

        /// <summary>
        /// Liste des propriétaires en DTO
        /// </summary>
        public List<ProprietaireDTO> GetListeProprietaires()
        {
            try
            {
                var proprietaires = db.proprietaires.ToList();
                return proprietaires.ToDTO();
            }
            catch (Exception ex)
            {
                helper.WriteDataError("Service1-GetListeProprietaires", ex.ToString());
                return new List<ProprietaireDTO>();
            }
        }

        public List<AppartementDTO> GetListeAppartement1()
        {
            var liste = db.appartements.Include(a => a.Proprietaire).ToList();
            return liste.ToDTO();
        }

        public async Task<List<ProprietaireDTO>> GetListeProprietaires_v1Async()
        {
            using (var db = new BdAppartementContext())
            {
                return await db.proprietaires
                    .Select(p => new ProprietaireDTO
                    {
                        IdPersonne = p.IdPersonne,
                        NomPrenom = p.NomPrenom
                    })
                    .ToListAsync();
            }
        }

        // Dans MetierGestion/Service1.svc.cs
        public async Task<List<AppartementDTO>> GetListeAppartement_v1Async(
            string filtreAdresse,
            float? filtreSurfaceMin,
            bool? filtreDisponible)
        {
            using (var db = new BdAppartementContext())
            {
                try
                {
                    var query = db.appartements.AsQueryable();

                    // Filtres optionnels
                    if (!string.IsNullOrEmpty(filtreAdresse))
                        query = query.Where(a => a.AdresseAppartement.Contains(filtreAdresse));

                    if (filtreSurfaceMin.HasValue)
                        query = query.Where(a => a.Surface >= filtreSurfaceMin);

                    if (filtreDisponible.HasValue)
                        query = query.Where(a => a.Disponible == filtreDisponible);

                    // Conversion en DTO
                    var result = await query
                        .Select(a => new AppartementDTO
                        {
                            IdAppartement = a.IdAppartement,
                            AdresseAppartement = a.AdresseAppartement,
                            Surface = a.Surface,
                            NombrePiece = a.NombrePiece,
                            Disponible = a.Disponible,
                            IdProprietaire = a.IdProprietaire,
                            NomPrenomProprietaire = a.Proprietaire.NomPrenom // Jointure implicite
                        })
                        .ToListAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    // Loggez l'erreur (ex: via votre classe Helper)
                    helper.WriteDataError("GetListeAppartement_v1Async", ex.ToString());
                    throw new FaultException($"Erreur lors de la récupération des appartements : {ex.Message}");
                }
            }
        }

        public void AjouterProprietaire(ProprietaireDTO p)
        {
            using (BdAppartementContext db = new BdAppartementContext())
            {
                Proprietaire prop = new Proprietaire
                {
                    NomPrenom = p.NomPrenom,
                    Telephone = p.Telephone,
                    Email = p.Email,
                    Ninea = p.Ninea,
                    Rccm = p.Rccm
                };

                db.proprietaires.Add(prop);
                db.SaveChanges();
            }
        }

        public void ModifierProprietaire(ProprietaireDTO p)
        {
            using (BdAppartementContext db = new BdAppartementContext())
            {
                var prop = db.proprietaires.Find(p.IdPersonne);
                if (prop != null)
                {
                    prop.NomPrenom = p.NomPrenom;
                    prop.Telephone = p.Telephone;
                    prop.Email = p.Email;
                    prop.Ninea = p.Ninea;
                    prop.Rccm = p.Rccm;

                    db.SaveChanges();
                }
            }
        }

        public void SupprimerProprietaire(int id)
        {
            using (BdAppartementContext db = new BdAppartementContext())
            {
                var prop = db.proprietaires.Find(id);
                if (prop != null)
                {
                    db.proprietaires.Remove(prop);
                    db.SaveChanges();
                }
            }
        }

        public List<AppartementDTO> GetAppartementsByProprietaire(int idProprietaire)
        {
            using (BdAppartementContext db = new BdAppartementContext())
            {
                return db.appartements.Where(a => a.Proprietaire.IdPersonne == idProprietaire)
                                      .Select(a => new AppartementDTO
                                      {
                                          IdAppartement = a.IdAppartement,
                                          AdresseAppartement = a.AdresseAppartement,
                                          Surface = a.Surface,
                                          // autres champs nécessaires
                                      })
                                      .ToList();
            }
        }

        public List<LocataireDTO> GetListeLocataires()
        {
            using (var db = new BdAppartementContext())
            {
                return db.locataires.Select(l => new LocataireDTO
                {
                    IdPersonne = l.IdPersonne,
                    NomPrenom = l.NomPrenom,
                    Telephone = l.Telephone,
                    Email = l.Email,
                    CNI = l.CNI
                }).ToList();
            }
        }

        public void AjouterLocataire(LocataireDTO l)
        {
            using (var db = new BdAppartementContext())
            {
                Locataire locataire = new Locataire
                {
                    NomPrenom = l.NomPrenom,
                    Telephone = l.Telephone,
                    Email = l.Email,
                    CNI = l.CNI
                };
                db.locataires.Add(locataire);
                db.SaveChanges();
            }
        }

        public void ModifierLocataire(LocataireDTO l)
        {
            using (var db = new BdAppartementContext())
            {
                var locataire = db.locataires.Find(l.IdPersonne);
                if (locataire != null)
                {
                    locataire.NomPrenom = l.NomPrenom;
                    locataire.Telephone = l.Telephone;
                    locataire.Email = l.Email;
                    locataire.CNI = l.CNI;
                    db.SaveChanges();
                }
            }
        }

        public void SupprimerLocataire(int id)
        {
            using (var db = new BdAppartementContext())
            {
                var locataire = db.locataires.Find(id);
                if (locataire != null)
                {
                    db.locataires.Remove(locataire);
                    db.SaveChanges();
                }
            }
        }

        //Contrat location

        public bool AjouterContratLocation(ContratLocationDTO dto)
        {
            using (var db = new BdAppartementContext())
            {
                var contrat = new ContratLocation
                {
                    Numero = dto.Numero,
                    DateDebut = dto.DateDebut,
                    DateFin = dto.DateFin,
                    DateCreation = dto.DateCreation,
                    Montant = dto.Montant,
                    statut = dto.statut,
                    IdAppartement = dto.IdAppartement,
                    IdLocataire = dto.IdLocataire,
                    IdPaiement = dto.IdPaiement
                };
                db.contratLocations.Add(contrat);
                db.SaveChanges();
                return true;
            }
        }

        public bool ModifierContratLocation(ContratLocationDTO dto)
        {
            using (var db = new BdAppartementContext())
            {
                var contrat = db.contratLocations.Find(dto.IdContrat);
                if (contrat == null) return false;

                contrat.Numero = dto.Numero;
                contrat.DateDebut = dto.DateDebut;
                contrat.DateFin = dto.DateFin;
                contrat.DateCreation = dto.DateCreation;
                contrat.Montant = dto.Montant;
                contrat.statut = dto.statut;
                contrat.IdAppartement = dto.IdAppartement;
                contrat.IdLocataire = dto.IdLocataire;
                contrat.IdPaiement = dto.IdPaiement;

                db.SaveChanges();
                return true;
            }
        }

        public bool SupprimerContratLocation(int id)
        {
            using (var db = new BdAppartementContext())
            {
                var contrat = db.contratLocations.Find(id);
                if (contrat == null) return false;
                db.contratLocations.Remove(contrat);
                db.SaveChanges();
                return true;
            }
        }

        public List<ContratLocationDTO> GetListeContrats()
        {
            using (var db = new BdAppartementContext())
            {
                return db.contratLocations.Select(c => new ContratLocationDTO
                {
                    IdContrat = c.IdContrat,
                    Numero = c.Numero,
                    DateDebut = c.DateDebut,
                    DateFin = c.DateFin,
                    DateCreation = c.DateCreation,
                    Montant = c.Montant,
                    statut = c.statut,
                    IdAppartement = c.IdAppartement,
                    IdLocataire = c.IdLocataire,
                    IdPaiement = c.IdPaiement
                }).ToList();
            }
        }

        public ContratLocationDTO GetContratById(int id)
        {
            using (var db = new BdAppartementContext())
            {
                var c = db.contratLocations.Find(id);
                if (c == null) return null;

                return new ContratLocationDTO
                {
                    IdContrat = c.IdContrat,
                    Numero = c.Numero,
                    DateDebut = c.DateDebut,
                    DateFin = c.DateFin,
                    DateCreation = c.DateCreation,
                    Montant = c.Montant,
                    statut = c.statut,
                    IdAppartement = c.IdAppartement,
                    IdLocataire = c.IdLocataire,
                    IdPaiement = c.IdPaiement
                };
            }
        }


        public bool AjouterUtilisateur(UtilisateurDTO utilisateur)
        {
            try
            {
                using (var db = new BdAppartementContext())
                {
                    var u = new Utilisateur
                    {
                        NomPrenom = utilisateur.NomPrenom,
                        Email = utilisateur.Email,
                        Telephone = utilisateur.Telephone,
                        Identifiant = utilisateur.Identifiant,
                        MotDePasse = utilisateur.MotDePasse,
                        Statut = utilisateur.Statut ?? "Actif"
                    };
                    db.Utilisateurs.Add(u);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<UtilisateurDTO> GetUtilisateurs()
        {
            using (var db = new BdAppartementContext())
            {
                return db.Utilisateurs.Select(u => new UtilisateurDTO
                {
                    IdPersonne = u.IdPersonne,
                    NomPrenom = u.NomPrenom,
                    Email = u.Email,
                    Telephone = u.Telephone,
                    Identifiant = u.Identifiant,
                    Statut = u.Statut
                }).ToList();
            }
        }

        public UtilisateurDTO GetUtilisateurByIdentifiant(string identifiant)
        {
            try
            {
                using (var db = new BdAppartementContext())
                {
                    // Requête plus robuste avec gestion des espaces et casse
                    var normalizedIdentifiant = identifiant?.Trim().ToUpper();
                    var user = db.Utilisateurs
                        .Where(u => u.Identifiant.ToUpper() == normalizedIdentifiant)
                        .Select(u => new UtilisateurDTO
                        {
                            IdPersonne = u.IdPersonne,
                            NomPrenom = u.NomPrenom,
                            Email = u.Email,
                            Identifiant = u.Identifiant,
                            MotDePasse = u.MotDePasse,
                            Statut = u.Statut //?? "Disabled"
                        })
                        .FirstOrDefault();

                    if (user == null)
                    {
                        System.Diagnostics.Trace.TraceWarning($"Identifiant non trouvé: {identifiant}");
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Erreur GetUtilisateurByIdentifiant: {ex}");
                throw new FaultException("Erreur lors de l'authentification");
            }
        }

        // Récupérer l'administrateur
        public AdminDTO GetAdmin()
        {
            var admin = db.admin.FirstOrDefault();
            if (admin != null)
            {
                return new AdminDTO
                {
                    IdPersonne = admin.IdPersonne,
                    NomPrenom = admin.NomPrenom,
                    Telephone = admin.Telephone,
                    Identifiant = admin.Identifiant,
                    Email = admin.Email
                };
            }
            return null;
        }

        // Créer un nouvel administrateur
        public bool CreateAdmin(AdminDTO newAdmin)
        {
            try
            {
                var admin = new Admin
                {
                    NomPrenom = newAdmin.NomPrenom,
                    Telephone = newAdmin.Telephone,
                    Identifiant = newAdmin.Identifiant,
                    Email = newAdmin.Email
                };
                db.admin.Add(admin);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ResetPassword(int idUtilisateur, string nouveauMotDePasse)
        {
            try
            {
                var utilisateur = db.Utilisateurs.Find(idUtilisateur);
                if (utilisateur != null)
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        utilisateur.MotDePasse = CryptApp.GetMd5Hash(md5Hash, nouveauMotDePasse);
                    }
                    utilisateur.Statut = "Enabled";
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool EstEmailUnique(string email)
        {
            return !db.Utilisateurs.Any(u => u.Email.ToLower() == email.ToLower());
        }

        public bool EstIdentifiantUnique(string identifiant)
        {
            return !db.Utilisateurs.Any(u => u.Identifiant.ToLower() == identifiant.ToLower());
        }

        public bool EstTelephoneUnique(string telephone)
        {
            return !db.Utilisateurs.Any(u => u.Telephone == telephone);
        }

        public UtilisateurDTO VerifierUtilisateur(string identifiant)
        {
            var user = db.Utilisateurs
                .FirstOrDefault(a => a.Identifiant.ToLower() == identifiant.ToLower() || a.Email.ToLower() == identifiant.ToLower());

            if (user == null) return null;

            return new UtilisateurDTO
            {
                IdPersonne = user.IdPersonne,
                NomPrenom = user.NomPrenom,
                Email = user.Email,
                Telephone = user.Telephone,
                Identifiant = user.Identifiant,
                MotDePasse = user.MotDePasse,
                Statut = user.Statut
            };
        }

        /// <summary>
        /// cette methode permet de logger les erreurs dans la table td_erreur
        /// </summary>
        /// <param name="TitreErreur">titre erreur</param>
        /// <param name="erreur">erreur</param>
        public void WriteDataError(string TitreErreur, string erreur)
        {
            try
            {
                Td_Erreur log = new Td_Erreur();
                log.DateErreur = DateTime.Now;
                log.DescriptionErreur = erreur.Length > 2000 ? erreur.Substring(0, 2000) : erreur;
                log.TitreErreur = TitreErreur;
                db.td_Erreurs.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                WriteLogSystem(ex.ToString(), "WriteDataError");
            }
        }

        public static void WriteLogSystem(string erreur, string libelle)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Gestion Appartement";
                eventLog.WriteEntry(string.Format("date: {0}, libelle: {1}, description {2}", DateTime.Now, libelle, erreur), EventLogEntryType.Information, 101, 1);
            }
        }

    }
}
