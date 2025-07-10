using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using Gestion.Utils;
using Gestion.View;
using System.Collections.Generic;
using System.Net.Http;

namespace Gestion.View
{
    public partial class frmResetPassword : Form
    {
        public int idUser;
        //Service1Client client = new Service1Client();

        public frmResetPassword()
        {
            InitializeComponent();
        }

        //private void btnValider_Click(object sender, EventArgs e)
        //{
        //    if (txtMotDePasse.Text == txtConfirmerMotDePasse.Text)
        //    {
        //        bool ok = client.ResetPassword(idUser, txtMotDePasse.Text);
        //        if (ok)
        //        {
        //            frmMDI f = new frmMDI();
        //            f.Show();
        //            this.Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Erreur lors de la mise à jour du mot de passe.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Les mots de passe ne sont pas identiques.");
        //    }
        //}

        private void btnValider_Click(object sender, EventArgs e)
        {
            string motDePasse = txtMotDePasse.Text.Trim();
            string confirmer = txtConfirmerMotDePasse.Text.Trim();

            if (motDePasse != confirmer)
            {
                MessageBox.Show("Les mots de passe ne sont pas identiques.");
                return;
            }

            bool ok = ResetPasswordApi(idUser, motDePasse);

            if (ok)
            {
                frmMDI f = new frmMDI();
                f.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour du mot de passe.");
            }
        }


        private void btnQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Appel API
        private bool ResetPasswordApi(int id, string nouveauMotDePasse)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var values = new Dictionary<string, string>
        {
            { "IdPersonne", id.ToString() },
            { "NouveauMotDePasse", nouveauMotDePasse }
        };

                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync("utilisateurs/reset-password", content).Result;

                return response.IsSuccessStatusCode;
            }
        }

        #endregion
    }
}
