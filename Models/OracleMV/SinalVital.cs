using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SINAL_VITAL")]
    public class SinalVital
    {

        [Key, Column("CD_SINAL_VITAL")]
        public decimal cdSinalVital { get; set; }

        [Column("DS_SINAL_VITAL")]
        public string dsSinalVital { get; set; }
    }
}
