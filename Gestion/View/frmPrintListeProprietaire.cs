using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mysqlx.Notice.Warning.Types;
using Gestion.Report;
using System.Web.Services.Description;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Gestion.Model;


namespace Gestion.View
{
    public partial class frmPrintListeProprietaire : Form
    {
        public frmPrintListeProprietaire()
        {
            InitializeComponent();
        }

        private readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8000/api/") 
        };

        //BdAppartementContext db= new BdAppartementContext();

        //Service1Client service = new Service1Client();
        
        //private void frmPrintListeProprietaire_Load(object sender, EventArgs e)
        //{
        //    rptListePropietaire objRpt = new rptListePropietaire();
        //    objRpt.SetDataSource(GetTableProprietaire());
        //    crystalReportViewer1.ReportSource = objRpt;
        //    crystalReportViewer1.Refresh();
        //}

        private async void frmPrintListeProprietaire_Load(object sender, EventArgs e)
        {
            var table = await GetTableProprietaire();
            rptListePropietaire objRpt = new rptListePropietaire();
            objRpt.SetDataSource(table);
            crystalReportViewer1.ReportSource = objRpt;
            crystalReportViewer1.Refresh();
        }


        //public DataTable GetTableProprietaire()
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add("Ninea", typeof(string));
        //    table.Columns.Add("Rccm", typeof(string));
        //    table.Columns.Add("NomPrenom", typeof(string));
        //    table.Columns.Add("Telephone", typeof(string));
        //    table.Columns.Add("Email", typeof(string));
        //    //var App = db.proprietaires.ToList();

        //    var proprietaires = service.GetListeProprietaires();

        //    foreach (var i in proprietaires)
        //    {
        //        table.Rows.Add(i.Ninea, i.Rccm, i.NomPrenom, i.Telephone, i.Email);
        //    }

        //    return table;
        //}

        public async Task<DataTable> GetTableProprietaire()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Ninea", typeof(string));
            table.Columns.Add("Rccm", typeof(string));
            table.Columns.Add("NomPrenom", typeof(string));
            table.Columns.Add("Telephone", typeof(string));
            table.Columns.Add("Email", typeof(string));

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("proprietaires");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var proprietaires = JsonConvert.DeserializeObject<List<Proprietaire>>(responseBody);

                foreach (var p in proprietaires)
                {
                    table.Rows.Add(p.Ninea, p.Rccm, p.NomPrenom, p.Telephone, p.Email);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des propriétaires : " + ex.Message);
            }

            return table;
        }


        private void frmPrintListeProprietaire_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmProprietaire f=new frmProprietaire();
            f.Activer();
        }
    }
}
