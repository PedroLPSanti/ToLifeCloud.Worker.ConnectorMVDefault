using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("TRIAG_ATENDIME_HIST_PAGU_AVAL")]
    public class TriagAtendimeHistPaguAval
    {
        [Column("CD_AVALIACAO")]
        public decimal cdAvaliacao { get; set; }

        [Column("CD_TRIAGEM_ATENDIMENTO_HIST")]
        public decimal cdTriagemAtendimentoHist { get; set; }
    }
}
