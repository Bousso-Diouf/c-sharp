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
using Gestion.MetierGestion;

namespace Gestion.View
{
    public partial class frmPrintListeProprietaire : Form
    {
        public frmPrintListeProprietaire()
        {
            InitializeComponent();
        }
        //BdAppartementContext db= new BdAppartementContext();

        Service1Client service = new Service1Client();
        private void frmPrintListeProprietaire_Load(object sender, EventArgs e)
        {
            rptListePropietaire objRpt = new rptListePropietaire();
            objRpt.SetDataSource(GetTableProprietaire());
            crystalReportViewer1.ReportSource = objRpt;
            crystalReportViewer1.Refresh();
        }

        public DataTable GetTableProprietaire()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Ninea", typeof(string));
            table.Columns.Add("Rccm", typeof(string));
            table.Columns.Add("NomPrenom", typeof(string));
            table.Columns.Add("Telephone", typeof(string));
            table.Columns.Add("Email", typeof(string));
            //var App = db.proprietaires.ToList();

            var proprietaires = service.GetListeProprietaires();

            foreach (var i in proprietaires)
            {
                table.Rows.Add(i.Ninea, i.Rccm, i.NomPrenom, i.Telephone, i.Email);
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
