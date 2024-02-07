using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("PRESTADOR")]
    public class Prestador
    {
        [Key]
        [Column("cd_prestador")]
        public decimal cdPrestador { get; set; }

        [Column("cd_cbos")]
        public decimal cdCbos { get; set; }

        [Column("cd_conselho")]
        public decimal cdConselho { get; set; }

        [Column("cd_tip_presta")]
        public decimal cdTipPresta { get; set; }

        [Column("ds_codigo_conselho")]
        public string dsCodigoConselho { get; set; }

        [Column("nm_mnemonico")]
        public string nmMnemonico { get; set; }

        [Column("nm_prestador")]
        public string nmPrestador { get; set; }

        [Column("nr_cpf_cgc")]
        public string nrCpfCgc { get; set; }

    }
}
