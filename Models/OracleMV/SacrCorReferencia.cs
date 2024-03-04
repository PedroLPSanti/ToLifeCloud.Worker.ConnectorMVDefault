using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_COR_REFERENCIA")]
    public class SacrCorReferencia
    {
        [Key, Column("CD_COR_REFERENCIA")]
        public decimal cdCorReferencia { get; set; }

        [Column("NM_COR")]
        public decimal nmCor { get; set; }
    }
}
