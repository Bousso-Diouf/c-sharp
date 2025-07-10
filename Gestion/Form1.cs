using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using Gestion.Utils;
using Gestion.View;
using Gestion.MetierGestion;
using Gestion;
using Gestion.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

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

        //private void btnSeConnecter_Click(object sender, EventArgs e)
        //{
        //    GMailer.senMail("elcheikh771@gmail.com", "Test", "Test envoie");
        //    Helper.WriteLogSystem("frmConnexion-btnSeConnecter_Click", "Bienvenue");

        //    try
        //    {
        //        if (!Utils.Validation.ChampsRemplis(txtIdentifiant.Text, txtMotDePasse.Text))
        //        {
        //            MessageBox.Show("Veuillez remplir tous les champs.");
        //            return;
        //        }

        //        string identifiant = txtIdentifiant.Text.Trim();
        //        var leuser = client.GetUtilisateurByIdentifiant(identifiant);

        //        if (leuser != null)
        //        {
        //            bool isFirstConnexion = string.IsNullOrEmpty(leuser.Statut);

        //            // Cas particulier : Admin en première connexion → mot de passe ignoré
        //            if (identifiant.Equals("admin", StringComparison.OrdinalIgnoreCase) && isFirstConnexion)
        //            {
        //                frmResetPassword f = new frmResetPassword();
        //                f.idUser = leuser.IdPersonne;
        //                f.Show();
        //                this.Hide();
        //                return;
        //            }

        //            // Vérification classique du mot de passe
        //            string hash = leuser.MotDePasse;
        //            using (MD5 md5Hash = MD5.Create())
        //            {
        //                if (CryptApp.VerifyMd5Hash(md5Hash, txtMotDePasse.Text, hash))
        //                {
        //                    if (isFirstConnexion)
        //                    {
        //                        frmResetPassword f = new frmResetPassword();
        //                        f.idUser = leuser.IdPersonne;
        //                        f.Show();
        //                        this.Hide();
        //                    }
        //                    else
        //                    {
        //                        frmMDI frm = new frmMDI();

        //                        if (identifiant.Equals("admin", StringComparison.OrdinalIgnoreCase))
        //                        {
        //                            frm.profil = "Admin";
        //                            frm.Show();
        //                            this.Hide();

        //                        }
        //                        else
        //                        {
        //                            frm.profil = "Gestionnaire";
        //                            frm.Show();
        //                            this.Hide();
        //                        }
        //                        //else
        //                        //{

        //                        //}
        //                        //frm.Show();
        //                        //this.Hide();
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Identifiant ou mot de passe incorrect.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Identifiant ou mot de passe incorrect.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.WriteLogSystem(ex.ToString(), "btnSeConnecter_Click");
        //        MessageBox.Show("Une erreur est survenue. Veuillez consulter les logs.");
        //    }
        //}

        private void btnSeConnecter_Click(object sender, EventArgs e)
        {
            Helper.WriteLogSystem("frmConnexion-btnSeConnecter_Click", "Tentative de connexion");

            try
            {
                if (!Utils.Validation.ChampsRemplis(txtIdentifiant.Text, txtMotDePasse.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs.");
                    return;
                }

                string identifiant = txtIdentifiant.Text.Trim();
                string motDePasse = txtMotDePasse.Text.Trim();

                // 1. Appel API REST : GET /utilisateurs/identifiant
                Utilisateur leuser = LoginViaApi(identifiant, motDePasse);
                //MessageBox.Show("Identifiant : " + leuser.Identifiant);


                if (leuser != null)
                {
                    bool isFirstConnexion = string.IsNullOrEmpty(leuser.Statut);

                    // Cas particulier : admin première connexion → pas de mot de passe requis
                    if (identifiant.Equals("admin", StringComparison.OrdinalIgnoreCase) && isFirstConnexion)
                    {
                        frmResetPassword f = new frmResetPassword();
                        f.idUser = leuser.IdPersonne;
                        f.Show();
                        this.Hide();
                        return;
                    }

                    // Vérification locale du mot de passe MD5
                    using (MD5 md5Hash = MD5.Create())
                    {
                        if (CryptApp.VerifyMd5Hash(md5Hash, motDePasse, leuser.MotDePasse))
                        {
                            //MessageBox.Show("Mot de passe : " + motDePasse + "\nleuser mot de passe : " + leuser.MotDePasse);

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
                                }
                                else
                                {
                                    frm.profil = "Gestionnaire";
                                }

                                frm.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Identifiant ou mot de passe incorrect. 1");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Identifiant ou mot de passe incorrect. 2");
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLogSystem(ex.ToString(), "btnSeConnecter_Click");
                MessageBox.Show("Une erreur est survenue. Veuillez consulter les logs.");
            }
        }

        private Utilisateur GetUtilisateurByIdentifiant(string identifiant)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("utilisateurs").Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var utilisateurs = JsonConvert.DeserializeObject<List<Utilisateur>>(json);

                    return utilisateurs.FirstOrDefault(u => u.Identifiant.Equals(identifiant, StringComparison.OrdinalIgnoreCase));
                }

                return null;
            }
        }

        private Utilisateur LoginViaApi(string identifiant, string motDePasse)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var values = new Dictionary<string, string>
                {
                    { "Identifiant", identifiant },
                    { "MotDePasse", motDePasse }
                };

                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync("utilisateurs/login", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<Utilisateur>(json);
                }

                return null;
            }
        }

    }
}
