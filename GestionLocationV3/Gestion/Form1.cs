using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using Gestion.Utils;
using Gestion.View;
using Gestion.MetierGestion;
using Gestion;

namespace Gestion
{
    public partial class frmConnexion : Form
    {
        //BdAppartementContext db = new BdAppartementContext(); // Supprimé car non utilisé (WCF now)
        Service1Client client = new Service1Client();

        public frmConnexion()
        {
            InitializeComponent();
        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSeConnecter_Click(object sender, EventArgs e)
        {
            GMailer.senMail("elcheikh771@gmail.com", "Test", "Test envoie");
            Helper.WriteLogSystem("frmConnexion-btnSeConnecter_Click", "Bienvenue");

            try
            {
                if (!Utils.Validation.ChampsRemplis(txtIdentifiant.Text, txtMotDePasse.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs.");
                    return;
                }

                string identifiant = txtIdentifiant.Text.Trim();
                var leuser = client.GetUtilisateurByIdentifiant(identifiant);

                if (leuser != null)
                {
                    bool isFirstConnexion = string.IsNullOrEmpty(leuser.Statut);

                    // Cas particulier : Admin en première connexion → mot de passe ignoré
                    if (identifiant.Equals("admin", StringComparison.OrdinalIgnoreCase) && isFirstConnexion)
                    {
                        frmResetPassword f = new frmResetPassword();
                        f.idUser = leuser.IdPersonne;
                        f.Show();
                        this.Hide();
                        return;
                    }

                    // Vérification classique du mot de passe
                    string hash = leuser.MotDePasse;
                    using (MD5 md5Hash = MD5.Create())
                    {
                        if (CryptApp.VerifyMd5Hash(md5Hash, txtMotDePasse.Text, hash))
                        {
                            if (isFirstConnexion)
                            {
                                frmResetPassword f = new frmResetPassword();
                                f.idUser = leuser.IdPersonne;
                                f.Show();
                                this.Hide();
                            }
                            else
                            {
                                frmMDI frm = new frmMDI();

                                if (identifiant.Equals("admin", StringComparison.OrdinalIgnoreCase))
                                {
                                    frm.profil = "Admin";
                                    frm.Show();
                                    this.Hide();

                                }
                                else
                                {
                                    frm.profil = "Gestionnaire";
                                    frm.Show();
                                    this.Hide();
                                }
                                //else
                                //{
                                    
                                //}
                                //frm.Show();
                                //this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Identifiant ou mot de passe incorrect.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Identifiant ou mot de passe incorrect.");
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLogSystem(ex.ToString(), "btnSeConnecter_Click");
                MessageBox.Show("Une erreur est survenue. Veuillez consulter les logs.");
            }
        }
    }
}
