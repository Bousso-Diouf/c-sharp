using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using MetierGestion.Model;
using MetierGestion.Model.DTO;

namespace MetierGestion
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        bool AddAppartement(AppartementDTO appartement);

        [OperationContract]
        bool UpdateAppartement(AppartementDTO appartement);

        [OperationContract]
        bool DeleteAppartement(int idAppartement);

        [OperationContract]
        AppartementDTO GetAppartementById(int? id);

        [OperationContract]
        List<AppartementDTO> GetListeAppartement(string adresse, float? capacite, bool? disponible);

        [OperationContract]
        List<ProprietaireDTO> GetListeProprietaires();

        [OperationContract]
        void AjouterProprietaire(ProprietaireDTO p);

        [OperationContract]
        void ModifierProprietaire(ProprietaireDTO p);

        [OperationContract]
        void SupprimerProprietaire(int id);

        [OperationContract]
        List<AppartementDTO> GetAppartementsByProprietaire(int idProprietaire);


        // Optionnel : méthode sans filtre
        [OperationContract]
        List<AppartementDTO> GetListeAppartement1();

        // Dans MetierGestion/IService1.cs
        [OperationContract]
        Task<List<ProprietaireDTO>> GetListeProprietaires_v1Async();

        [OperationContract]
        Task<List<AppartementDTO>> GetListeAppartement_v1Async(string filtreAdresse, float? filtreSurfaceMin, bool? filtreDisponible);

        // LOCATAIRES
        [OperationContract]
        List<LocataireDTO> GetListeLocataires();

        [OperationContract]
        void AjouterLocataire(LocataireDTO l);

        [OperationContract]
        void ModifierLocataire(LocataireDTO l);

        [OperationContract]
        void SupprimerLocataire(int id);

        //Contrat de location

        [OperationContract]
        bool AjouterContratLocation(ContratLocationDTO contrat);

        [OperationContract]
        bool ModifierContratLocation(ContratLocationDTO contrat);

        [OperationContract]
        bool SupprimerContratLocation(int id);

        [OperationContract]
        List<ContratLocationDTO> GetListeContrats();

        [OperationContract]
        ContratLocationDTO GetContratById(int id);

        [OperationContract]
        bool AjouterUtilisateur(UtilisateurDTO utilisateur);

        [OperationContract]
        List<UtilisateurDTO> GetUtilisateurs();

        [OperationContract]
        UtilisateurDTO GetUtilisateurByIdentifiant(string identifiant);

        [OperationContract]
        AdminDTO GetAdmin();

        [OperationContract]
        bool CreateAdmin(AdminDTO newAdmin);

        [OperationContract]
        bool ResetPassword(int idUtilisateur, string nouveauMotDePasse);

        [OperationContract]
        bool EstEmailUnique(string email);

        [OperationContract]
        bool EstIdentifiantUnique(string identifiant);

        [OperationContract]
        bool EstTelephoneUnique(string telephone);

        [OperationContract]
        UtilisateurDTO VerifierUtilisateur(string identifiant);

        [OperationContract]
        void WriteDataError(string TitreErreur, string erreur);



    }

    // Contrat de données d'exemple
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
