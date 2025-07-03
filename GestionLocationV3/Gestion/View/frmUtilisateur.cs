using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Gestion.MetierGestion;
using Gestion.Utils;

namespace Gestion.View
{
    public partial class frmUtilisateur : Form
    {
        public frmUtilisateur()
        {
            InitializeComponent();
        }

        Service1Client client = new Service1Client();

        private void btnAjouter_Click(object sender, EventArgs e)
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
            if (!client.EstEmailUnique(email))
            {
                MessageBox.Show("Cette adresse email est déjà utilisée.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. Téléphone unique
            if (!client.EstTelephoneUnique(telephone))
            {
                MessageBox.Show("Ce numéro de téléphone est déjà utilisé.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 6. Identifiant unique
            if (!client.EstIdentifiantUnique(identifiant))
            {
                MessageBox.Show("Cet identifiant est déjà utilisé.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 7. Tout est valide → Création
            using (MD5 md5Hash = MD5.Create())
            {
                string mdp = "P@sser123";
                string mdpHash = CryptApp.GetMd5Hash(md5Hash, mdp);

                UtilisateurDTO ut = new UtilisateurDTO
                {
                    NomPrenom = nomPrenom,
                    Email = email,
                    Telephone = Validation.NormaliserTelephone(telephone),
                    Identifiant = identifiant,
                    MotDePasse = mdpHash,
                    Statut = "Actif"
                };

                bool res = client.AjouterUtilisateur(ut);
                if (res)
                {
                    MessageBox.Show("Utilisateur ajouté !");
                    GMailer.senMail(ut.Email, "Création compte",
                        $"Bonjour,\nVotre compte est bien créé.\nIdentifiant: {ut.Identifiant}\nMot de passe: {mdp}");
                    ResetForm();
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

            var utilisateurs = client.GetUtilisateurs();
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
    }
}
