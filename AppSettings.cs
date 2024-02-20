using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public class AppSettings
    {
        public AppSettings()
        {
            workersDelays = new WorkersDelays();
        }

        public bool log { get; set; }
        public string urlApiSrvLog { get; set; }
        public string urlApiIntegration { get; set; }
        public string urlSRVMessageQueue { get; set; }
        public string urlIntegrationRelationConfig { get; set; }
        public string internalLoginHash { get; set; }
        public bool logKeepAliveWorkerRun { get; set; }
        public bool ticketReaderWorkerRun { get; set; }
        public bool callTicketWorkerRun { get; set; }
        public bool patientTriageWorkerRun { get; set; }
        public bool updateConfigWorkerRun { get; set; }
        public bool ticketEvasionWorkerRun { get; set; }
        public long idHealthUnit { get; set; }
        public WorkersDelays workersDelays { get; set; }
        public int timezone { get; set; }
    }

    public class WorkersDelays
    {
        public int ticketReader { get; set; }

        public int patientTriage { get; set; }

        public int callTicket { get; set; }

        public int updateConfig { get; set; }

        public int logKeepAlive { get; set; }

        public int ticketEvasion { get; set; }
    }
}
