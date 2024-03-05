using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Responses
{
    public class MessageQueueResponse : ReturnStruct
    {
        public string? message { get; set; }
        public string? idMessageType { get; set; }

        public bool ValidValue()
        {
            bool value;
            return (!isError && !string.IsNullOrEmpty(message) && Boolean.TryParse(message, out value) && value);
        }
    }
}