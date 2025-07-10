using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gestion.Model;

namespace Gestion.View
{
    public partial class frmProduit: Form
    {
        public frmProduit()
        {
            InitializeComponent();
        }

        private void frmProduit_Load(object sender, EventArgs e)
        {
            dgProduit.DataSource = servGetListProduit();
        }

        #region Appel API
        public List<Produit> servGetListProduit()
        {
            HttpClient client;
            client = new HttpClient();
            var services = new List<Produit>();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationSettings.AppSettings["ServeurApiURL"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["gl2021/Values/GetEmployees"]).Result;
            var response = client.GetAsync("groupe1/Produit/GetAllProduits").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                services = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Produit>>(responseData);
            }
            return services;
        }

        public bool AddProduit(Produit emp)
        {
            bool rep = false;
            string Id = emp.IdProduit > 0 ? emp.IdProduit.ToString() : "0";
            var values = new Dictionary<string, string>
                    {
                       { "IdProduit", Id },
                       { "CodeProduit", emp.CodeProduit },
                       { "DesignationProduit", emp.DesignationProduit },
                       { "PU", emp.PU.ToString() },
                       { "QteMin", emp.QteMin.ToString() }
                    };
            var content = new FormUrlEncodedContent(values);
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(System.Configuration.ConfigurationSettings.AppSettings["ServeurApiURL"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsync("groupe1/Produit/AddProduit", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        rep = true;
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {

            }
            return rep;
        }

        #endregion

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            Produit p = new Produit();
            //p.IdProduit = Convert.ToInt32(txtId.Text);
            p.CodeProduit = txtCode.Text;
            p.DesignationProduit = txtDesignation.Text;
            //p.QteMin = string.IsNullOrEmpty(txtQuantiteMinimale.Text) ? (int?)null : Convert.ToInt32(txtQuantiteMinimale.Text);
            p.QteMin = string.IsNullOrEmpty(txtQuantiteMinimale.Text) ? (int?)null : int.Parse(txtQuantiteMinimale.Text);
            //p.PU = string.IsNullOrEmpty(txtPU.Text) ? (decimal?)null : Convert.ToDecimal(txtPU.Text);
            p.PU = string.IsNullOrEmpty(txtPU.Text) ? (int?)null : int.Parse(txtPU.Text);

            AddProduit(p);

            frmProduit_Load(sender, e);

        }

    }
}
