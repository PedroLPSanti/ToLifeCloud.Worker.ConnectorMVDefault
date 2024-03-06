using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_CLASSIFICACAO_RISCO")]
    public class SacrClassificacaoRisco
    {
        public SacrClassificacaoRisco()
        {
        }
        public SacrClassificacaoRisco(TriagemAtendimento triagemAtendimento)
        {
            cdCorReferencia = triagemAtendimento.cdCorReferencia.Value;
            cdSintomaAvaliacao = triagemAtendimento.cdSintomaAvaliacao;
            cdClassificacao = triagemAtendimento.cdClassificacao.Value;
            dsObservacao = triagemAtendimento.dsObservacao;
            cdTriagemAtendimento = triagemAtendimento.cdTriagemAtendimento;
            dhClassificacaoRisco = triagemAtendimento.dhPreAtendimentoFim.Value;
            vlEscore = 0;
        }
        [Key, Column("CD_CLASSIFICACAO_RISCO")]
        public decimal cdClassificacaoRisco { get; set; }

        [Column("CD_COR_REFERENCIA")]
        public decimal cdCorReferencia { get; set; }

        [Column("CD_SINTOMA_AVALIACAO")]
        public decimal? cdSintomaAvaliacao { get; set; }

        [Column("CD_CLASSIFICACAO")]
        public decimal cdClassificacao { get; set; }

        [Column("DS_OBSERVACAO")]
        public string dsObservacao { get; set; }

        [Column("CD_TRIAGEM_ATENDIMENTO")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("DH_CLASSIFICACAO_RISCO")]
        public DateTime dhClassificacaoRisco { get; set; }

        [Column("VL_ESCORE")]
        public decimal vlEscore { get; set; }
    }
}
