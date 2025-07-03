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
    public partial class frmProprietaire : Form
    {

        MetierGestion.Service1Client client = new MetierGestion.Service1Client();


        public frmProprietaire()
        {
            InitializeComponent();
        }

        private void ResetForm()
        {
            txtEmail.Text = string.Empty;
            txtNinea.Text = string.Empty;
            txtNomPrenom.Text = string.Empty;
            txtRccm.Text = string.Empty;
            txtTel.Text = string.Empty;

            try
            {
                var liste = client.GetListeProprietaires();
                dgProprietaire.DataSource = liste.Select(p => new
                {
                    p.IdPersonne,
                    p.NomPrenom,
                    p.Telephone,
                    p.Email,
                    p.Ninea,
                    p.Rccm
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement : " + ex.Message);
            }

            txtNomPrenom.Focus();
        }

        private void frmProprietaire_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                ProprietaireDTO p = new ProprietaireDTO
                {
                    NomPrenom = txtNomPrenom.Text,
                    Rccm = txtRccm.Text,
                    Telephone = txtTel.Text,
                    Email = txtEmail.Text,
                    Ninea = txtNinea.Text
                };

                client.AjouterProprietaire(p);
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout : " + ex.Message);
            }
        }

        private void btnChoisir_Click(object sender, EventArgs e)
        {
            if (dgProprietaire.CurrentRow != null)
            {
                txtNomPrenom.Text = dgProprietaire.CurrentRow.Cells[1].Value.ToString();
                txtTel.Text = dgProprietaire.CurrentRow.Cells[2].Value.ToString();
                txtEmail.Text = dgProprietaire.CurrentRow.Cells[3].Value.ToString();
                txtNinea.Text = dgProprietaire.CurrentRow.Cells[4].Value.ToString();
                txtRccm.Text = dgProprietaire.CurrentRow.Cells[5].Value.ToString();
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgProprietaire.CurrentRow != null)
            {
                try
                {
                    int id = int.Parse(dgProprietaire.CurrentRow.Cells[0].Value.ToString());
                    ProprietaireDTO p = new ProprietaireDTO
                    {
                        IdPersonne = id,
                        NomPrenom = txtNomPrenom.Text,
                        Rccm = txtRccm.Text,
                        Telephone = txtTel.Text,
                        Email = txtEmail.Text,
                        Ninea = txtNinea.Text
                    };

                    client.ModifierProprietaire(p);
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification : " + ex.Message);
                }
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgProprietaire.CurrentRow != null)
            {
                try
                {
                    int id = int.Parse(dgProprietaire.CurrentRow.Cells[0].Value.ToString());

                    // Vérifier si le propriétaire a des appartements associés
                    bool hasAppartements = client.GetAppartementsByProprietaire(id).Any();

                    if (hasAppartements)
                    {
                        MessageBox.Show("Le propriétaire a des appartements associés. Suppression impossible.");
                    }
                    else
                    {
                        client.SupprimerProprietaire(id);
                        ResetForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                }
            }
        }


        private void btnImprimer_Click(object sender, EventArgs e)
        {
            frmPrintListeProprietaire f = new frmPrintListeProprietaire();
            f.Show();
            this.Enabled = false;
        }

        public void Activer()
        {
            this.Enabled = true;
        }
    }
}
