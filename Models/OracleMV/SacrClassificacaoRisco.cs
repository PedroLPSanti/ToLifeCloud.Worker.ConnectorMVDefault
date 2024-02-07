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
            cdCorReferencia = triagemAtendimento.cdCorReferencia;
            cdSintomaAvaliacao = triagemAtendimento.cdSintomaAvaliacao;
            cdClassificacao = triagemAtendimento.cdClassificacao;
            dsObservacao = triagemAtendimento.dsObservacao;
            cdTriagemAtendimento = triagemAtendimento.cdTriagemAtendimento;
            dhClassificacaoRisco = triagemAtendimento.dhPreAtendimentoFim;
            vlEscore = 0;
        }
        [Key, Column("cd_classificacao_risco")]
        public decimal cdClassificacaoRisco { get; set; }

        [Column("cd_cor_referencia")]
        public decimal cdCorReferencia { get; set; }

        [Column("cd_sintoma_avaliacao")]
        public decimal cdSintomaAvaliacao { get; set; }

        [Column("cd_classificacao")]
        public decimal cdClassificacao { get; set; }

        [Column("ds_observacao")]
        public string dsObservacao { get; set; }

        [Column("cd_triagem_atendimento")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("dh_classificacao_risco")]
        public DateTime dhClassificacaoRisco { get; set; }

        [Column("vl_escore")]
        public decimal vlEscore { get; set; }
    }
}
