using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Structs
{
    public class ReturnStruct
    {
        public bool isError { get; set; }

        public int errorCode { get; set; }

        public string errorDescription { get; set; }
    }
}
