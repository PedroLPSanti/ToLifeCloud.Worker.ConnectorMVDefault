using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_CLASSIFICACAO")]
    public class SacrClassificacao
    {
        [Key, Column("CD_CLASSIFICACAO")]
        public decimal cdClassificacao { get; set; }
        [Column("CD_PROTOCOLO")]
        public decimal cdProtocolo { get; set; }
        [Column("DS_TIPO_RISCO")]
        public decimal dsTipoRisco { get; set; }
        [Column("CD_COR_REFERENCIA")]
        public decimal cdCorReferencia { get; set; }
    }
}
