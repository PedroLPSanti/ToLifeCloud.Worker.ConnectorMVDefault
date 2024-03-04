using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("TRIAGEM_ATENDIMENTO_HISTORICO")]
    public class TriagemAtendimentoHist
    {
        public TriagemAtendimentoHist()
        {
        }
        public TriagemAtendimentoHist(TriagemAtendimento triagemAtendimento, bool isReclassification, ListVariableStruct variables, TriageWebhookStruct triageWebhook)
        {
            cdCorReferencia = triagemAtendimento.cdCorReferencia;
            cdSintomaAvaliacao = triagemAtendimento.cdSintomaAvaliacao;
            cdClassificacao = triagemAtendimento.cdClassificacao;
            dsQueixaPrincipal = triagemAtendimento.dsQueixaPrincipal;
            cdEspecialid = triagemAtendimento.cdEspecialid;
            dsAlergia = triagemAtendimento.dsAlergia;
            dsObservacao = triagemAtendimento.dsObservacao;
            cdUsuario = triagemAtendimento.cdUsuario;
            nmUsuarioTriagem = triagemAtendimento.nmUsuarioTriagem;
            vlEscore = triagemAtendimento.vlEscore;
            tpClassificacao = triagemAtendimento.tpClassificacao;
            cdTriagemAtendimento = triagemAtendimento.cdTriagemAtendimento;
            dhInicio = triageWebhook.startClassification.Value.AddHours(3);
            dhFim = triagemAtendimento.dhPreAtendimentoFim;
            dhTriagemAtendimentoHist = DateTime.Now;
            if (isReclassification)
            {
                cdTriagemAtendimeHistTip = variables.getVariable<decimal>(VariableTypeEnum.hist_reclassificacao_tip);
            }
            else
            {
                cdTriagemAtendimeHistTip = variables.getVariable<decimal>(VariableTypeEnum.hist_classificacao_tip);
            }
            tpAlergia = string.IsNullOrEmpty(triagemAtendimento.dsAlergia) ? 'N' : 'S';
        }

        [Key, Column("CD_TRIAGEM_ATENDIMENTO_HIST")]
        public decimal cdTriagemAtendimentoHist { get; set; }

        [Column("CD_TRIAGEM_ATENDIMENTO")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("CD_ESPECIALID")]
        public decimal? cdEspecialid { get; set; }

        [Column("CD_CLASSIFICACAO")]
        public decimal? cdClassificacao { get; set; }

        [Column("NM_USUARIO_TRIAGEM")]
        public string? nmUsuarioTriagem { get; set; }

        [Column("DH_TRIAGEM_ATENDIMENTO_HIST")]
        public DateTime? dhTriagemAtendimentoHist { get; set; }

        [Column("VL_ESCORE")]
        public decimal? vlEscore { get; set; }

        [Column("CD_SINTOMA_AVALIACAO")]
        public decimal? cdSintomaAvaliacao { get; set; }

        [Column("CD_COR_REFERENCIA")]
        public decimal? cdCorReferencia { get; set; }

        [Column("DS_QUEIXA_PRINCIPAL")]
        public string? dsQueixaPrincipal { get; set; }

        [Column("DS_ALERGIA")]
        public string? dsAlergia { get; set; }

        [Column("DS_OBSERVACAO")]
        public string? dsObservacao { get; set; }

        [Column("DH_INICIO")]
        public DateTime? dhInicio { get; set; }

        [Column("DH_FIM")]
        public DateTime? dhFim { get; set; }

        [Column("CD_USUARIO")]
        public string cdUsuario { get; set; }

        [Column("CD_TRIAGEM_ATENDIME_HIST_TIP")]
        public decimal? cdTriagemAtendimeHistTip { get; set; }

        [Column("TP_CLASSIFICACAO")]
        public string? tpClassificacao { get; set; }

        [Column("TP_ALERGIA")]
        public char? tpAlergia { get; set; }
    }
}
