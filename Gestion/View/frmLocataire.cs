using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gestion.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Gestion.Model;


namespace Gestion.View
{
    //public partial class frmLocataire : Form
    //{
    //    // Client WCF
    //    Service1Client client = new Service1Client();

    //    public frmLocataire()
    //    {
    //        InitializeComponent();
    //    }

    //    private void frmLocataire_Load(object sender, EventArgs e)
    //    {
    //        ResetForm();
    //    }

    //    private void ResetForm()
    //    {
    //        txtEmail.Text = string.Empty;
    //        txtNomPrenom.Text = string.Empty;
    //        txtCNI.Text = string.Empty;
    //        txtTel.Text = string.Empty;

    //        var liste = client.GetListeLocataires();
    //        dgLocataire.DataSource = liste.Select(l => new
    //        {
    //            l.IdPersonne,
    //            l.NomPrenom,
    //            l.Telephone,
    //            l.Email,
    //            l.CNI
    //        }).ToList();

    //        txtNomPrenom.Focus();
    //    }

    //    private void btnAjouter_Click(object sender, EventArgs e)
    //    {
    //        LocataireDTO locataire = new LocataireDTO
    //        {
    //            NomPrenom = txtNomPrenom.Text,
    //            CNI = txtCNI.Text,
    //            Telephone = txtTel.Text,
    //            Email = txtEmail.Text
    //        };

    //        client.AjouterLocataire(locataire);
    //        ResetForm();
    //    }

    //    private void btnChoisir_Click(object sender, EventArgs e)
    //    {
    //        if (dgLocataire.CurrentRow != null)
    //        {
    //            txtNomPrenom.Text = dgLocataire.CurrentRow.Cells["NomPrenom"].Value.ToString();
    //            txtTel.Text = dgLocataire.CurrentRow.Cells["Telephone"].Value.ToString();
    //            txtEmail.Text = dgLocataire.CurrentRow.Cells["Email"].Value.ToString();
    //            txtCNI.Text = dgLocataire.CurrentRow.Cells["CNI"].Value.ToString();
    //        }
    //    }

    //    private void btnModifier_Click(object sender, EventArgs e)
    //    {
    //        if (dgLocataire.CurrentRow != null)
    //        {
    //            int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);
    //            LocataireDTO locataire = new LocataireDTO
    //            {
    //                IdPersonne = id,
    //                NomPrenom = txtNomPrenom.Text,
    //                CNI = txtCNI.Text,
    //                Telephone = txtTel.Text,
    //                Email = txtEmail.Text
    //            };

    //            client.ModifierLocataire(locataire);
    //            ResetForm();
    //        }
    //    }

    //    private void btnSupprimer_Click(object sender, EventArgs e)
    //    {
    //        if (dgLocataire.CurrentRow != null)
    //        {
    //            int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);
    //            client.SupprimerLocataire(id);
    //            ResetForm();
    //        }
    //    }

    //    private void btnContrat_Click(object sender, EventArgs e)
    //    {
    //        if (dgLocataire.CurrentRow != null)
    //        {
    //            int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);

    //            frmContratLocation frm = new frmContratLocation();
    //            frm.Appartement = "Adresse"; // à adapter selon ton implémentation
    //            frm.Show();
    //        }
    //    }
    //}

    public partial class frmLocataire : Form
    {
        private readonly HttpClient httpClient = new HttpClient();

        public frmLocataire()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:8000/api/"); // adapte au besoin
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void frmLocataire_Load(object sender, EventArgs e)
        {
            await ResetForm();
        }

        private async Task ResetForm()
        {
            txtEmail.Text = "";
            txtNomPrenom.Text = "";
            txtCNI.Text = "";
            txtTel.Text = "";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("locataires");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var locataires = JsonSerializer.Deserialize<List<Locataire>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    dgLocataire.DataSource = locataires.Select(l => new
                    {
                        l.IdPersonne,
                        l.NomPrenom,
                        l.Telephone,
                        l.Email,
                        l.CNI
                    }).ToList();
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement des locataires.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur réseau : {ex.Message}");
            }

            txtNomPrenom.Focus();
        }

        private async void btnAjouter_Click(object sender, EventArgs e)
        {
            var locataire = new Locataire
            {
                NomPrenom = txtNomPrenom.Text,
                Telephone = txtTel.Text,
                Email = txtEmail.Text,
                CNI = txtCNI.Text
            };

            var content = new StringContent(JsonSerializer.Serialize(locataire), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("locataires", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Ajout réussi !");
                await ResetForm();
            }
            else
            {
                MessageBox.Show("Échec de l'ajout.");
            }
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

        private async void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgLocataire.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);
                var locataire = new Locataire
                {
                    NomPrenom = txtNomPrenom.Text,
                    Telephone = txtTel.Text,
                    Email = txtEmail.Text,
                    CNI = txtCNI.Text
                };

                var content = new StringContent(JsonSerializer.Serialize(locataire), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"locataires/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Modification réussie !");
                    await ResetForm();
                }
                else
                {
                    MessageBox.Show("Échec de la modification.");
                }
            }
        }

        private async void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgLocataire.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgLocataire.CurrentRow.Cells["IdPersonne"].Value);

                HttpResponseMessage response = await httpClient.DeleteAsync($"locataires/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Suppression réussie !");
                    await ResetForm();
                }
                else
                {
                    MessageBox.Show("Échec de la suppression.");
                }
            }
        }

        private void btnContrat_Click(object sender, EventArgs e)
        {
            frmContratLocation frm = new frmContratLocation();
            frm.Appartement = "Adresse"; // à adapter
            frm.Show();
        }
    }

}
