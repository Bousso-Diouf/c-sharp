using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace serWinGroupe1
{
    public partial class Service1 : ServiceBase
     
    {
        private static System.Timers.Timer aTimer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            aTimer = new Timer(1000);

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            aTimer.Interval = 1000;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;

            WriteLogSystem("Demarrage du service ", string.Format("Le service a démarré à {0}", DateTime.Now));

        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                ProcessData().Wait();
            }
            catch (Exception ex)
            {
            }


            //aTimer.Start();
        }
        private static async Task ProcessData()
        {
            try
            {

                try
                {
                    WriteLogSystem("execution du service", string.Format("date et heure", DateTime.Now));
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {
                WriteLogSystem(ex.ToString(), "SendMail");
            }
        }
        


        protected override void OnStop()
        {
            aTimer.Stop();
            aTimer.Dispose();
            WriteLogSystem("Arrêt du service ", string.Format("Le service est arrêté à {0}", DateTime.Now));

        }
        public static void WriteLogSystem(string erreur, string libelle)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "servive windows groupe1";
                eventLog.WriteEntry(string.Format("date: {0}, libelle: {1}, description {2}", DateTime.Now, libelle, erreur), EventLogEntryType.Information, 101, 1);
            }
        }





    }
}
