using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("PW_SINAL_VITAL_UNID_INSTR_AFER")]
    public class PwSinalVitalUnidInstrAfer
    {
        [Key, Column("CD_SINAL_VITAL_UNID_INSTR_AFER")]
        public decimal cdSinalVitalUnidInstrAfer { get; set; }

        [Column("CD_INSTRUMENTO_AFERICAO")]
        public decimal cdInstrumentoAfericao { get; set; }

        [Column("CD_UNIDADE_AFERICAO")]
        public decimal cdUnidadeAfericao { get; set; }

        [Column("TP_LANCAMENTO")]
        public string tpLancamento { get; set; }

        [Column("CD_SINAL_VITAL")]
        public decimal cdSinalVital { get; set; }
    }
}
