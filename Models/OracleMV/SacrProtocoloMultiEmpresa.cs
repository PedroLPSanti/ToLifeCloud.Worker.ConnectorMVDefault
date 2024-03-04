using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_PROTOCOLO_MULTI_EMPRESA")]
    public class SacrProtocoloMultiEmpresa
    {
        [Key, Column("CD_PROTOCOLO_MULTI_EMPRESA")]
        public string cdProtocoloMultiEmpresa { get; set; }

        [Column("CD_PROTOCOLO")]
        public decimal cdProtocolo { get; set; }

        [Column("CD_MULTI_EMPRESA")]
        public decimal cdMultiEmpresa { get; set; }
    }
}
