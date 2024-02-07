using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Responses
{    
    public class MessageQueueResponse : ReturnStruct
    {
        public string? message { get; set; }
        public string? idMessageType { get; set; }
    }
}