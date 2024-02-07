using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Requests
{
    public class MessageQueueRequest
    {
        [Required]
        public string message { get; set; }

        public string? idMessageType { get; set; }
    }
}