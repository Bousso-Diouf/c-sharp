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
    public partial class frmContratLocation : Form
    {
        public frmContratLocation()
        {
            InitializeComponent();
        }

        Service1Client client = new Service1Client();

        public string Appartement; // Tu peux aussi passer l'ID si nécessaire

        private void frmContratLocation_Load(object sender, EventArgs e)
        {
            lblAppartement.Text = Appartement;

            // Tu peux charger un contrat spécifique ici si besoin
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            // Exemple de création ou modification d’un contrat (à adapter selon les champs que tu as)
            ContratLocationDTO contrat = new ContratLocationDTO
            {
                IdContrat = 1, // ou null si ajout
                Numero = "CL-2025-001",
                DateDebut = DateTime.Now,
                DateFin = DateTime.Now.AddMonths(12),
                DateCreation = DateTime.Now,
                Montant = 30000,
                statut = "Actif",
                IdAppartement = 1,
                IdLocataire = 1,
                IdPaiement = null
            };

            bool success = client.ModifierContratLocation(contrat);
            if (success)
            {
                MessageBox.Show("Contrat mis à jour !");
            }
            else
            {
                MessageBox.Show("Échec !");
            }
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {

        }
    }
}
