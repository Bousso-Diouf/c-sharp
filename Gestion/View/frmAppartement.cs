using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gestion.Utils;

namespace Gestion.View
{
    public partial class frmAppartement : Form
    {
        public frmAppartement()
        {
            InitializeComponent();
        }

        MetierGestion.Service1Client service = new MetierGestion.Service1Client();
        //BdAppartementContext db=new BdAppartementContext();
        Helper helper = new Helper();

        private void ResetForm()
        {
            txtAdresse.Text = string.Empty;
            txtCapacite.Text = string.Empty;
            txtNombrePiece.Text = string.Empty;
            txtSurface.Text = string.Empty;
            cbbDisponible.Text = "Selectionnez...";

            // Recharger les propriétaires
            //cbbProprietaire.DataSource = LoadCbbProprietaire().ToList();
            cbbProprietaire.DisplayMember = "NomPrenom";
            cbbProprietaire.ValueMember = "IdPersonne";

            // Charger les appartements via le service WCF
            dgAppartement.DataSource = service.GetListeAppartement(null, null, null)
                .Select(a => new
                {
                    a.IdAppartement,
                    a.IdProprietaire,
                    a.NomPrenomProprietaire,  // Modifié ici pour correspondre au DTO
                    a.NombrePiece,
                    a.Capacite,
                    a.Surface,
                    a.Disponible
                })
                .ToList();

            txtAdresse.Focus();
        }


        //private List<ListSelectionViewModel> LoadCbbProprietaire()
        //{
        //    List<ListSelectionViewModel> list = new List<ListSelectionViewModel>();

        //    try
        //    {
        //        var listeProprietaires = service.GetListeProprietaires();

        //        // Ajout de l'option "Selectionnez...."
        //        list.Add(new ListSelectionViewModel
        //        {
        //            Text = "Selectionnez....",
        //            Value = string.Empty
        //        });

        //        foreach (var item in listeProprietaires)
        //        {
        //            list.Add(new ListSelectionViewModel
        //            {
        //                Text = item.NomPrenom,
        //                Value = item.IdPersonne.ToString()
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Erreur lors du chargement des propriétaires : " + ex.Message);
        //    }

        //    return list;
        //}

        //private List<Model.ListSelectionViewModel> LoadCbbProprietaire()
        //{
        //    using (var db = new Model.BdAppartementContext())
        //    {
        //        var proprietaires = db.proprietaires.ToList();

        //        var list = new List<Model.ListSelectionViewModel>
        //        {
        //            new Model.ListSelectionViewModel { Text = "Sélectionner...", Value = "" }
        //        };

        //        list.AddRange(proprietaires.Select(p => new Model.ListSelectionViewModel
        //        {
        //            Text = p.NomPrenom,
        //            Value = p.IdPersonne.ToString()
        //        }));

        //        return list;
        //    }
        //}


        //private void frmAppartement_Load(object sender, EventArgs e)
        //{
        //    ResetForm();
        //}

        private async void frmAppartement_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Chargement des propriétaires via le service métier
                var proprietaires = await service.GetListeProprietaires_v1Async(); // Utilisez votre méthode existante

                cbbProprietaire.DataSource = proprietaires
                    .Select(p => new { p.IdPersonne, p.NomPrenom })
                    .ToList();
                cbbProprietaire.DisplayMember = "NomPrenom";
                cbbProprietaire.ValueMember = "IdPersonne";

                // 2. Chargement des appartements via le service métier
                var appartements = await service.GetListeAppartement_v1Async(null, null, null); // Adaptez aux paramètres réels

                dgAppartement.DataSource = appartements
                    .Select(a => new
                    {
                        a.IdAppartement,
                        a.AdresseAppartement,
                        a.Surface,
                        Proprietaire = a.NomPrenomProprietaire, // Champ DTO
                        a.Disponible
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                //helper.WriteDataError("frmAppartement_Load", ex.ToString());
                service.WriteDataError("frmAppartement_Load", ex.ToString());
                MessageBox.Show("Erreur lors du chargement des données initiales.");
            }
            finally
            {
                txtAdresse.Focus();
            }
        }

        //private void btnAjouter_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Validation des champs obligatoires
        //        if (string.IsNullOrWhiteSpace(txtAdresse.Text))
        //        {
        //            MessageBox.Show("Veuillez entrer une adresse valide.");
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(txtCapacite.Text) || !float.TryParse(txtCapacite.Text, out float capacite))
        //        {
        //            MessageBox.Show("Veuillez entrer une capacité valide.");
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(txtSurface.Text) || !float.TryParse(txtSurface.Text, out float surface))
        //        {
        //            MessageBox.Show("Veuillez entrer une surface valide.");
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(txtNombrePiece.Text) || !int.TryParse(txtNombrePiece.Text, out int nombrePiece))
        //        {
        //            MessageBox.Show("Veuillez entrer un nombre de pièces valide.");
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(cbbProprietaire.SelectedValue?.ToString()) ||
        //            !db.proprietaires.Any(p => p.IdPersonne == int.Parse(cbbProprietaire.SelectedValue.ToString())))
        //        {
        //            MessageBox.Show("Veuillez sélectionner un propriétaire valide.");
        //            return;
        //        }

        //        // Création de l'objet AppartementDTO
        //        MetierGestion.AppartementDTO a = new MetierGestion.AppartementDTO
        //        {
        //            AdresseAppartement = txtAdresse.Text,
        //            Capacite = capacite,
        //            Surface = surface,
        //            NombrePiece = nombrePiece,
        //            Disponible = cbbDisponible.Text == "Oui",
        //            IdProprietaire = int.Parse(cbbProprietaire.SelectedValue.ToString())
        //        };

        //        // Appel du service pour ajouter l'appartement
        //        bool result = service.AddAppartement(a);

        //        if (result)
        //        {
        //            MessageBox.Show("Appartement ajouté avec succès !");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Erreur lors de l’ajout de l’appartement.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Gestion des exceptions
        //        if (ex is DbUpdateException dbEx)
        //        {
        //            var innerMessage = dbEx.InnerException?.InnerException?.Message ?? dbEx.InnerException?.Message;
        //            helper.WriteDataError("frmAppartement-btnAjouter_Click", $"DbUpdateException: {innerMessage}");
        //        }
        //        else
        //        {
        //            helper.WriteDataError("frmAppartement-btnAjouter_Click", ex.ToString());
        //        }

        //        MessageBox.Show("Une erreur est survenue lors de l'ajout de l'appartement. Veuillez vérifier les données saisies.");
        //    }
        //    finally
        //    {
        //        // Réinitialisation du formulaire
        //        ResetForm();
        //    }
        //}

        private async void btnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validation des champs
                if (string.IsNullOrWhiteSpace(txtAdresse.Text))
                {
                    MessageBox.Show("L'adresse est obligatoire.");
                    return;
                }

                if (!float.TryParse(txtSurface.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out float surface) || surface <= 0)
                {
                    MessageBox.Show("Surface invalide. Utilisez un nombre positif (ex: 50.5)");
                    return;
                }

                if (!float.TryParse(txtCapacite.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out float capacite) || capacite <= 0)
                {
                    MessageBox.Show("Capacité invalide.");
                    return;
                }

                if (!int.TryParse(txtNombrePiece.Text, out int nombrePiece) || nombrePiece <= 0)
                {
                    MessageBox.Show("Nombre de pièces invalide.");
                    return;
                }

                // 2. Validation du propriétaire (sans accès direct à EF)
                if (!(cbbProprietaire.SelectedValue is int idProprietaire) || idProprietaire <= 0)
                {
                    MessageBox.Show("Sélectionnez un propriétaire valide.");
                    return;
                }

                // 3. Mapping vers le DTO
                var appartementDto = new MetierGestion.AppartementDTO
                {
                    AdresseAppartement = txtAdresse.Text.Trim(),
                    Surface = surface,
                    Capacite = capacite,
                    NombrePiece = nombrePiece,
                    Disponible = cbbDisponible.Text.Equals("Oui", StringComparison.OrdinalIgnoreCase),
                    IdProprietaire = idProprietaire
                };

                // 4. Appel asynchrone au service
                bool result = await Task.Run(() => service.AddAppartement(appartementDto));

                MessageBox.Show(result
                    ? "Appartement ajouté avec succès !"
                    : "Échec de l'ajout. Vérifiez les données.");

                // 5. Rechargement asynchrone
                await ChargerDonneesAsync();
            }
            catch (FaultException ex)
            {
                service.WriteDataError("btnAjouter_Click", $"Erreur service: {ex.Message}");
                MessageBox.Show($"Erreur du service : {ex.Message}");
            }
            catch (Exception ex)
            {
                service.WriteDataError("btnAjouter_Click", ex.ToString());
                MessageBox.Show("Erreur inattendue. Consultez les logs.");
            }
        }

        private async Task ChargerDonneesAsync()
        {
            try
            {
                var proprietaires = await service.GetListeProprietaires_v1Async();
                cbbProprietaire.DataSource = proprietaires
                    .Select(p => new { p.IdPersonne, p.NomPrenom })
                    .ToList();

                var appartements = await service.GetListeAppartement_v1Async(null, null, null);
                dgAppartement.DataSource = appartements
                    .Select(a => new
                    {
                        a.IdAppartement,
                        a.AdresseAppartement,
                        a.Surface,
                        Proprietaire = a.NomPrenomProprietaire,
                        a.Disponible
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                service.WriteDataError("ChargerDonneesAsync", ex.ToString());
            }
        }

        //private void btnModifier_Click(object sender, EventArgs e)
        //{
        //    int? id = int.Parse(dgAppartement.CurrentRow.Cells[0].Value.ToString());
        //    var a = service.GetAppartementById(id);
        //    a.Capacite = float.Parse(txtCapacite.Text);
        //    a.Disponible = cbbDisponible.Text == "Oui";
        //    a.Surface = float.Parse(txtSurface.Text);
        //    a.NombrePiece = int.Parse(txtNombrePiece.Text);
        //    a.AdresseAppartement = txtAdresse.Text;
        //    a.IdProprietaire = int.Parse(cbbProprietaire.SelectedValue.ToString());

        //    service.UpdateAppartement(a);
        //    ResetForm();
        //}

        private async void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgAppartement.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un appartement à modifier.");
                return;
            }

            try
            {
                int id = int.Parse(dgAppartement.CurrentRow.Cells[0].Value.ToString());
                var a = await Task.Run(() => service.GetAppartementById(id));

                if (a == null)
                {
                    MessageBox.Show("Appartement introuvable.");
                    return;
                }

                a.Capacite = float.Parse(txtCapacite.Text);
                a.Disponible = cbbDisponible.Text == "Oui";
                a.Surface = float.Parse(txtSurface.Text);
                a.NombrePiece = int.Parse(txtNombrePiece.Text);
                a.AdresseAppartement = txtAdresse.Text;
                a.IdProprietaire = int.Parse(cbbProprietaire.SelectedValue.ToString());

                bool result = await Task.Run(() => service.UpdateAppartement(a));

                MessageBox.Show(result ? "Appartement modifié avec succès." : "Échec de la modification.");

                await ChargerDonneesAsync();
            }
            catch (Exception ex)
            {
                service.WriteDataError("btnModifier_Click", ex.ToString());
                MessageBox.Show("Erreur lors de la modification.");
            }
        }


        private void btnChoisir_Click(object sender, EventArgs e)
        {
            int? id = int.Parse(dgAppartement.CurrentRow.Cells[0].Value.ToString());
            var a = service.GetAppartementById(id); // Cette méthode doit retourner un AppartementDTO

            // Si l'objet retourné est un AppartementDTO
            txtAdresse.Text = a.AdresseAppartement;
            cbbProprietaire.SelectedValue = a.IdProprietaire;  // IdProprietaire directement
            txtCapacite.Text = a.Capacite != null ? a.Capacite.ToString() : string.Empty;
            txtNombrePiece.Text = a.NombrePiece != null ? a.NombrePiece.ToString() : string.Empty;
            txtSurface.Text = a.Surface.ToString();
            cbbDisponible.SelectedValue = (bool)a.Disponible ? "Oui" : "Non";  // Ou un autre format si nécessaire
        }


        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            string message = "Voulez-Vous supprimer ?";
            string title = "Fermer";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                int? id = int.Parse(dgAppartement.CurrentRow.Cells[0].Value.ToString());
                service.DeleteAppartement(id.Value);

                //service.DeleteAppartement(a);
                //db.appartements.Remove(a);
                //db.SaveChanges();
                ResetForm();
            }
            
        }

        private void btnContrat_Click(object sender, EventArgs e)
        {
            frmContratLocation frm = new frmContratLocation();
            frm.Appartement = string.Format("Adresse");
            frm.Show();
        }


    }
}
