using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_PROTOCOLO")]
    public class SacrProtocolo
    {
        [Key]
        [Column("CD_PROTOCOLO")]
        public decimal cdProtocolo { get; set; }

        [Column("DS_PROTOCOLO")]
        public string dsProtocolo { get; set; }
    }
}
