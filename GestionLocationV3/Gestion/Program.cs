using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gestion.MetierGestion;
using Gestion.Utils;

namespace Gestion
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CreateAdmin();
            Application.Run(new frmConnexion());
        }

        static void CreateAdmin()
        {
            // Utilisation du client WCF pour interagir avec le service
            Service1Client client = new Service1Client();

            // Vérifier si l'administrateur existe déjà
            var admin = client.GetAdmin(); // On suppose que cette méthode existe dans le service WCF et renvoie l'administrateur si existant

            if (admin == null)
            {
                // Si l'administrateur n'existe pas, on le crée
                AdminDTO newAdmin = new AdminDTO
                {
                    NomPrenom = "Administrateur",
                    Telephone = "7700000000",
                    Identifiant = "Admin",
                    Email = "admin@yopmail.com"
                };

                bool success = client.CreateAdmin(newAdmin); // Méthode WCF pour créer un nouvel administrateur

                if (success)
                {
                    MessageBox.Show("Administrateur créé avec succès.");
                }
                else
                {
                    MessageBox.Show("Échec de la création de l'administrateur.");
                }
            }
        }
    }
}
