using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Responses
{
    public class EpisodeResponse : ReturnStruct
    {
        public long idEpisode { get; set; }
        public long? idPatient { get; set; }
        public long idQueue { get; set; }
    }
}
