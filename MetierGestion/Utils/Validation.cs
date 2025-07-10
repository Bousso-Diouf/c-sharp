using System;
using System.Linq;
using System.Text.RegularExpressions;
using MetierGestion.Model;

namespace MetierGestion.Utils
{
    public static class Validation
    {
        public static bool ChampsRemplis(string identifiant, string motDePasse)
        {
            if (string.IsNullOrWhiteSpace(identifiant) || string.IsNullOrWhiteSpace(motDePasse))
            {
                return false;
            }
            return true;
        }

        public static Utilisateur VerifierUtilisateur(BdAppartementContext db, string identifiant)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            return db.Utilisateurs
                     .Where(a => a.Identifiant.ToLower() == identifiant.ToLower() || a.Email.ToLower() == identifiant.ToLower())
                     .FirstOrDefault();
        }

        public static bool EstEmailUnique(BdAppartementContext db, string email)
        {
            return !db.Utilisateurs.Any(u => u.Email.ToLower() == email.ToLower());
        }

        public static bool EstIdentifiantUnique(BdAppartementContext db, string identifiant)
        {
            return !db.Utilisateurs.Any(u => u.Identifiant.ToLower() == identifiant.ToLower());
        }

        public static bool EstTelephoneUnique(BdAppartementContext db, string telephone)
        {
            return !db.Utilisateurs.Any(u => u.Telephone == telephone);
        }

        public static bool EstEmailValide(string email, out string message)
        {
            string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                message = "L'adresse email n'est pas valide.";
                return false;
            }
            message = "";
            return true;
        }

        public static bool EstTelephoneValide(string telephone, out string message)
        {
            telephone = telephone.Trim();
            if (telephone.StartsWith("+221"))
            {
                telephone = telephone.Substring(4).Trim();
            }
            if (!Regex.IsMatch(telephone, "^(77|78|76|75|70)\\d{7}$"))
            {
                message = "Le numéro de téléphone doit être au format sénégalais (77, 78, 76, 75, 70 suivi de 7 chiffres).";
                return false;
            }
            message = "";
            return true;
        }


        public static string NormaliserTelephone(string telephone)
        {
            telephone = telephone.Trim();
            if (telephone.StartsWith("+221"))
            {
                telephone = telephone.Substring(4).Trim();
            }
            if (!Regex.IsMatch(telephone, "^(77|78|76|75|70)\\d{7}$"))
            {
                return telephone; // Garder tel quel si ce n'est pas un numéro sénégalais
            }
            return telephone;
        }

        public static bool EstMotDePasseValide(string motDePasse, out string message)
        {
            //string motDePassePattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";

            string motDePassePattern = "^(?!.([A-Za-z0-9])\\1{1})(?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])(?=.?[#?!@$%^&-]).{8,}$";


            if (!Regex.IsMatch(motDePasse, motDePassePattern))
            {
                message = "Le mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial.\nLe mot de passe est : " + motDePasse;
                return false;
            }
            message = "";
            return true;
        }
    }
}
