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

namespace Gestion.View
{
    public partial class frmLocataire : Form
    {
        // Client WCF
        Service1Client client = new Service1Client();

        public frmLocataire()
        {
            InitializeComponent();
        }

        private void frmLocataire_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtEmail.Text = string.Empty;
            txtNomPrenom.Text = string.Empty;
            txtCNI.Text = string.Empty;
            txtTel.Text = string.Empty;

            var liste = client.GetListeLocataires();
            dgLocataire.DataSource = liste.Select(l => new
            {
                l.IdPersonne,
                l.NomPrenom,
                l.Telephone,
                l.Email,
                l.CNI
            }).ToList();

            txtNomPrenom.Focus();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            LocataireDTO locataire = new LocataireDTO
            {
                NomPrenom = txtNomPrenom.Text,
                CNI = txtCNI.Text,
                Telephone = txtTel.Text,
                Email = txtEmail.Text
            };

            client.AjouterLocataire(locataire);
            ResetForm();
        }

        private void btnChoisir_Click(object sender, EventArgs e)
        {
            if (dgLocataire.CurrentRow != null)
            {
                txtNomPrenom.Text = dgLocataire.CurrentRow.Cells["NomPrenom"].Value.ToString();
                txtTel.Text = dgLocataire.CurrentRow.Cells["Telephone"].Value.ToString();
                txtEmail.Text = dgLocataire.CurrentRow.Cells["Email"].Value.ToString();
                txtCNI.Text = dgLocataire.CurrentRow.Cells["CNI"].Value.ToString();
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgLocataire.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);
                LocataireDTO locataire = new LocataireDTO
                {
                    IdPersonne = id,
                    NomPrenom = txtNomPrenom.Text,
                    CNI = txtCNI.Text,
                    Telephone = txtTel.Text,
                    Email = txtEmail.Text
                };

                client.ModifierLocataire(locataire);
                ResetForm();
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgLocataire.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);
                client.SupprimerLocataire(id);
                ResetForm();
            }
        }

        private void btnContrat_Click(object sender, EventArgs e)
        {
            if (dgLocataire.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);

                frmContratLocation frm = new frmContratLocation();
                frm.Appartement = "Adresse"; // à adapter selon ton implémentation
                frm.Show();
            }
        }
    }
}
