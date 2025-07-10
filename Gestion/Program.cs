using System;
using System.Windows.Forms;
//using Gestion.MetierGestion;
using System.Net.Http;
using System.Text;
using Gestion.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Gestion
{
    internal static class Program
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8000/api/") // Adapter si nécessaire
        };
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CreateAdmin().Wait();
            Application.Run(new frmConnexion());
        }

        //static void CreateAdmin()
        //{
        //    // Utilisation du client WCF pour interagir avec le service
        //    Service1Client client = new Service1Client();

        //    // Vérifier si l'administrateur existe déjà
        //    var admin = client.GetAdmin(); // On suppose que cette méthode existe dans le service WCF et renvoie l'administrateur si existant

        //    if (admin == null)
        //    {
        //        // Si l'administrateur n'existe pas, on le crée
        //        AdminDTO newAdmin = new AdminDTO
        //        {
        //            NomPrenom = "Administrateur",
        //            Telephone = "7700000000",
        //            Identifiant = "Admin",
        //            Email = "admin@yopmail.com"
        //        };

        //        bool success = client.CreateAdmin(newAdmin); // Méthode WCF pour créer un nouvel administrateur

        //        if (success)
        //        {
        //            MessageBox.Show("Administrateur créé avec succès.");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Échec de la création de l'administrateur.");
        //        }
        //    }
        //}

        static async Task CreateAdmin()
        {
            try
            {
                HttpResponseMessage getResponse = await client.GetAsync("utilisateurs/admin");
                if (getResponse.IsSuccessStatusCode)
                {
                    string json = await getResponse.Content.ReadAsStringAsync();

                    // Vérifie si le tableau retourné contient des éléments
                    var admins = JsonConvert.DeserializeObject<List<object>>(json);
                    if (admins != null && admins.Count > 0)
                    {
                        return; // Un admin existe déjà
                    }
                }

                var newAdmin = new
                {
                    NomPrenom = "Administrateur",
                    Telephone = "770182910",
                    Identifiant = "admin2",
                    Email = "admin2@yopmail.com",
                    MotDePasse = "motdepasse"
                };

                var content = new StringContent(JsonConvert.SerializeObject(newAdmin), Encoding.UTF8, "application/json");

                HttpResponseMessage postResponse = await client.PostAsync("utilisateurs/create-admin", content);
                if (postResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("Administrateur créé avec succès.");
                }
                else
                {
                    string error = await postResponse.Content.ReadAsStringAsync();
                    MessageBox.Show("Échec de la création de l'administrateur.\n" + error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création de l'administrateur : " + ex.Message);
            }
        }
    }
}
