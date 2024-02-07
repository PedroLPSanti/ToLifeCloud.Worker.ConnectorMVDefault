using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Structs
{
    public class PanelCallWebhookStruct
    {
        public int idService { get; set; }
        public int idSector { get; set; }
        public long idRoom { get; set; }
        public string roomName { get; set; }
        public string sectorName { get; set; }
        public string serviceName { get; set; }
        public long idEpisode { get; set; }
        public string name { get; set; }
        public string socialName { get; set; }
        public int ticketSequence { get; set; }
        public string ticketInitial { get; set; }
        public int idUserRequested { get; set; }
        public int idHealthUnit { get; set; }
    }
}
