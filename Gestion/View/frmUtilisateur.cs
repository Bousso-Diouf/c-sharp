using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Gestion.MetierGestion;
using Gestion.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Gestion.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Gestion.View
{
    public partial class frmUtilisateur : Form
    {
        public frmUtilisateur()
        {
            InitializeComponent();

        }

        //Service1Client client = new Service1Client();

        // Classe pour désérialiser la réponse JSON de l'API Laravel
        public class UniqueResponse
        {
            [JsonProperty("unique")]
            public bool Unique { get; set; }
        }

private async Task<bool> EstUniqueAsync(string url)
{
    using (HttpClient httpClient = new HttpClient())
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                UniqueResponse result = JsonConvert.DeserializeObject<UniqueResponse>(json);
                return result.Unique;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la vérification de l'unicité : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    return false;
}


        private async void btnAjouter_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string identifiant = txtIdentifiant.Text.Trim();
            string telephone = txtTel.Text.Trim();
            string nomPrenom = txtNomPrenom.Text.Trim();

            string message;

            // 1. Vérification champs requis
            if (string.IsNullOrWhiteSpace(nomPrenom) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(identifiant) ||
                string.IsNullOrWhiteSpace(telephone))
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Email valide
            if (!Validation.EstEmailValide(email, out message))
            {
                MessageBox.Show(message, "Email invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Téléphone valide
            if (!Validation.EstTelephoneValide(telephone, out message))
            {
                MessageBox.Show(message, "Téléphone invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Email unique
            bool emailUnique = await EstUniqueAsync($"http://localhost/api/utilisateurs/email/{email}");
            if (!emailUnique)
            {
                MessageBox.Show("Cette adresse email est déjà utilisée.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. Téléphone unique
            bool telephoneUnique = await EstUniqueAsync($"http://localhost/api/utilisateurs/telephone/{telephone}");
            if (!telephoneUnique)
            {
                MessageBox.Show("Ce numéro de téléphone est déjà utilisé.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 6. Identifiant unique
            bool identifiantUnique = await EstUniqueAsync($"http://localhost/api/utilisateurs/identifiant/{identifiant}");
            if (!identifiantUnique)
            {
                MessageBox.Show("Cet identifiant est déjà utilisé.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 7. Tout est valide → Création
            using (MD5 md5Hash = MD5.Create())
            {
                string mdp = "P@sser123";
                string mdpHash = CryptApp.GetMd5Hash(md5Hash, mdp);

                //Utilisateur ut = new Utilisateur
                //{
                //    NomPrenom = nomPrenom,
                //    Email = email,
                //    Telephone = Validation.NormaliserTelephone(telephone),
                //    Identifiant = identifiant,
                //    MotDePasse = mdpHash,
                //    Statut = "Actif"
                //};

                //bool res = AddUtilisateur(ut);
                //if (res)
                //{
                //    MessageBox.Show("Utilisateur ajouté !");
                //    GMailer.senMail(ut.Email, "Création compte",
                //        $"Bonjour,\nVotre compte est bien créé.\nIdentifiant: {ut.Identifiant}\nMot de passe: {mdp}");
                //    ResetForm();
                //}
                //else
                //{
                //    MessageBox.Show("Échec lors de l'ajout !");
                //}
                Utilisateur u = new Utilisateur
                {
                    NomPrenom = txtNomPrenom.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telephone = txtTel.Text.Trim(),
                    Identifiant = txtIdentifiant.Text.Trim(),
                    MotDePasse = mdpHash
                };

                if (AddUtilisateur(u))
                {
                    MessageBox.Show("Utilisateur ajouté !");
                    ResetForm(); // Recharge les utilisateurs et vide les champs
                }
                else
                {
                    MessageBox.Show("Échec lors de l'ajout !");
                }
            }
        }
        

        private void ResetForm()
        {
            txtEmail.Text = txtIdentifiant.Text = txtNomPrenom.Text = txtTel.Text = string.Empty;
            txtNomPrenom.Focus();

            var utilisateurs = GetUtilisateurs();
            dgUtilisateur.DataSource = utilisateurs.Select(u => new
            {
                u.IdPersonne,
                u.NomPrenom,
                u.Identifiant,
                u.Email,
                u.Telephone
            }).ToList();
        }

        private void txtEmail_MouseLeave(object sender, EventArgs e)
        {
            txtIdentifiant.Text = txtEmail.Text;
        }

        private void frmUtilisateur_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        #region Appel API
        public List<Utilisateur> GetUtilisateurs()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("utilisateurs").Result;

            if (response.IsSuccessStatusCode)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Utilisateur>>(responseData);
            }

            return new List<Utilisateur>();
        }

        public bool AddUtilisateur(Utilisateur user)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var values = new Dictionary<string, string>
        {
            { "NomPrenom", user.NomPrenom },
            { "Telephone", user.Telephone },
            { "Email", user.Email },
            { "Identifiant", user.Identifiant },
            { "MotDePasse", user.MotDePasse },
            { "Statut", user.Statut}
        };

                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = client.PostAsync("utilisateurs", content).Result;

                return response.IsSuccessStatusCode;
            }
        }

        public bool DeleteUtilisateur(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.DeleteAsync("utilisateurs/" + id).Result;
                return response.IsSuccessStatusCode;
            }
        }

        public bool UpdateUtilisateur(Utilisateur user)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var values = new Dictionary<string, string>
        {
            { "NomPrenom", user.NomPrenom },
            { "Telephone", user.Telephone },
            { "Email", user.Email },
            { "Identifiant", user.Identifiant },
            { "Statut", user.Statut ?? "Actif" }
        };

                var content = new FormUrlEncodedContent(values);
                var response = client.PutAsync("utilisateurs/" + user.IdPersonne, content).Result;

                return response.IsSuccessStatusCode;
            }
        }

        #endregion

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgUtilisateur.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgUtilisateur.CurrentRow.Cells["IdPersonne"].Value);
                var confirm = MessageBox.Show("Voulez-vous vraiment supprimer cet utilisateur ?", "Confirmation", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    if (DeleteUtilisateur(id))
                    {
                        MessageBox.Show("Utilisateur supprimé !");
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show("Échec de la suppression.");
                    }
                }
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgUtilisateur.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgUtilisateur.CurrentRow.Cells["IdPersonne"].Value);

                Utilisateur u = new Utilisateur
                {
                    IdPersonne = id,
                    NomPrenom = txtNomPrenom.Text,
                    Telephone = txtTel.Text,
                    Email = txtEmail.Text,
                    Identifiant = txtIdentifiant.Text,
                    Statut = "Actif"
                };

                if (UpdateUtilisateur(u))
                {
                    MessageBox.Show("Utilisateur modifié !");
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Échec de la modification.");
                }
            }
        }

        private void btnChoisir_Click(object sender, EventArgs e)
        {
            if (dgUtilisateur.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgUtilisateur.CurrentRow.Cells["IdPersonne"].Value);
                var utilisateur = GetUtilisateurs().FirstOrDefault(u => u.IdPersonne == id);

                if (utilisateur != null)
                {
                    txtNomPrenom.Text = utilisateur.NomPrenom;
                    txtTel.Text = utilisateur.Telephone;
                    txtEmail.Text = utilisateur.Email;
                    txtIdentifiant.Text = utilisateur.Identifiant;
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
