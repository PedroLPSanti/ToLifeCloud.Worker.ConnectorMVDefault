using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_PROTOCOLO")]
    public class SacrProtocolo
    {
        [Key]
        [Column("cd_protocolo")]
        public decimal cd_protocolo { get; set; }

        [Column("ds_protocolo")]
        public string ds_protocolo { get; set; }
    }
}
