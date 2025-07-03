using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using Gestion.Utils;
using Gestion.View;
using Gestion.MetierGestion;

namespace Gestion.View
{
    public partial class frmResetPassword : Form
    {
        public int idUser;
        Service1Client client = new Service1Client();

        public frmResetPassword()
        {
            InitializeComponent();
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (txtMotDePasse.Text == txtConfirmerMotDePasse.Text)
            {
                bool ok = client.ResetPassword(idUser, txtMotDePasse.Text);
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
            else
            {
                MessageBox.Show("Les mots de passe ne sont pas identiques.");
            }
        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
