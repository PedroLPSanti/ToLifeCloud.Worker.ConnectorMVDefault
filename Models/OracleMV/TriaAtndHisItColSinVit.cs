using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("TRIA_ATND_HIS_IT_COL_SIN_VIT")]
    public class TriaAtndHisItColSinVit
    {
        [Column("CD_TRIAGEM_ATENDIMENTO_HIST")]
        public decimal cdTriagemAtendimentoHist { get; set; }


        [Column("CD_COLETA_SINAL_VITAL")]
        public decimal cdColetaSinalVital { get; set; }

        [Column("CD_SINAL_VITAL")]
        public decimal cdSinalVital { get; set; }
    }
}
