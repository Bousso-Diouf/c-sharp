using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gestion.Model; // ta classe ContratLocation

namespace Gestion.View
{
    public partial class frmContratLocation : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        public int? IdContrat { get; set; } = null;  // null si nouveau contrat
        public int? IdAppartement { get; set; } = null;
        public int? IdLocataire { get; set; } = null;
        public string Appartement { get; internal set; }

        public frmContratLocation()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:8000/api/"); // adapte ici
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void frmContratLocation_Load(object sender, EventArgs e)
        {
            if (IdContrat.HasValue)
            {
                // Chargement du contrat existant
                await LoadContrat(IdContrat.Value);
            }
            else
            {
                // Nouveau contrat
                lblAppartement.Text = $"Appartement ID : {IdAppartement}";
            }
        }

        private async Task LoadContrat(int id)
        {
            var response = await httpClient.GetAsync($"contrats/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var contrat = JsonSerializer.Deserialize<ContratLocation>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (contrat != null)
                {
                    txtNumero.Text = contrat.Numero;
                    dtpDateDebut.Value = contrat.DateDebut ?? DateTime.Now;
                    dtpDateFin.Value = contrat.DateFin ?? DateTime.Now.AddMonths(12);
                    txtMontant.Text = contrat.Montant?.ToString() ?? "";
                    //txtStatut.Text = contrat.statut;
                    lblAppartement.Text = $"Appartement ID : {contrat.IdTypeAppartement}";
                    IdLocataire = contrat.IdLocataire;
                    IdAppartement = contrat.IdTypeAppartement;
                }
            }
            else
            {
                MessageBox.Show("Impossible de charger le contrat.");
            }
        }

        private async void btnValider_Click(object sender, EventArgs e)
        {
            var contrat = new ContratLocation
            {
                IdContrat = IdContrat ?? 0, // 0 ou null pour création selon API
                Numero = txtNumero.Text,
                DateDebut = dtpDateDebut.Value,
                DateFin = dtpDateFin.Value,
                DateCreation = DateTime.Now,
                Montant = float.TryParse(txtMontant.Text, out float montant) ? montant : 0,
                //statut = txtStatut.Text,
                IdTypeAppartement = IdAppartement,
                IdLocataire = IdLocataire,
                IdPaiement = null
            };

            var json = JsonSerializer.Serialize(contrat);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (IdContrat.HasValue && IdContrat.Value > 0)
            {
                // Mise à jour
                response = await httpClient.PutAsync($"contrats/{IdContrat.Value}", content);
            }
            else
            {
                // Création
                response = await httpClient.PostAsync("contrats", content);
            }

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Contrat enregistré avec succès !");
                this.Close(); // ou autre action
            }
            else
            {
                MessageBox.Show("Erreur lors de l'enregistrement du contrat.");
            }
        }
    }
}
