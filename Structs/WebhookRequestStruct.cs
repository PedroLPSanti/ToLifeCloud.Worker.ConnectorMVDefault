using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Structs
{
    public class WebhookRequestStruct
    {
        public WebhookRequestStruct()
        {
            body = new TriageWebhookStruct();
        }

        public TriageWebhookStruct body { get; set; }
    }
}
