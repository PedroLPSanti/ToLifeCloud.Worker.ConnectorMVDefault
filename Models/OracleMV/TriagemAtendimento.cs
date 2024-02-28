using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("TRIAGEM_ATENDIMENTO")]
    public class TriagemAtendimento
    {
        public TriagemAtendimento()
        {
        }
        public TriagemAtendimento(TriageWebhookStruct triageWebhook, Usuarios usuarios, ListVariableStruct variables, Paciente paciente)
        {
            dhPreAtendimento = triageWebhook.startClassification.Value.AddHours(3);
            cdPaciente = paciente != null ? paciente.cdPaciente : null;
            nmPaciente = triageWebhook.patientName;
            dtNascimento = triageWebhook.patientBirthDate.HasValue ? triageWebhook.patientBirthDate.Value : null;
            tpSexo = string.IsNullOrEmpty(triageWebhook.patientGender) ? null : variables.getVariable<char>(VariableTypeEnum.sexo, triageWebhook.patientGender);
            cdCorReferencia = variables.getVariable<decimal>(VariableTypeEnum.cor_referencia, triageWebhook.idGravity);
            cdSintomaAvaliacao = variables.getVariable<decimal>(VariableTypeEnum.sintoma_avaliacao, triageWebhook.idFlowchart.Value);
            cdClassificacao = variables.getVariable<decimal>(VariableTypeEnum.classificacao, triageWebhook.idGravity);
            cdMultiEmpresa = variables.getVariable<decimal>(VariableTypeEnum.multi_empresa);
            vlIdade = triageWebhook.patientBirthDate.HasValue ? (int)Math.Floor((DateTime.Now - triageWebhook.patientBirthDate.Value).TotalDays / 365.2425) : null;
            dhPreAtendimentoFim = triageWebhook.endClassification.Value.AddHours(3);
            dsQueixaPrincipal = "Queixa :" + triageWebhook.complaint + "| Discriminador selecionado: " + triageWebhook.discriminator;
            cdEspecialid = variables.getVariable<decimal?>(VariableTypeEnum.especialid, triageWebhook.idForward.Value);
            dsAlergia = triageWebhook.allergy;
            dsObservacao = (triageWebhook.atmosphericAir.HasValue ? (triageWebhook.atmosphericAir.Value ? "Ar atmosférico" : "Em terapia de O2") + " | " : "")
                + (triageWebhook.heartRateRegular.HasValue ? "Batimentos cardíacos: " + (triageWebhook.heartRateRegular.Value ? "Regular" : "Irregular") + " | " : "") +
                (!string.IsNullOrEmpty(triageWebhook.diseaseHistory) ? "Histórico de doenças:" + triageWebhook.diseaseHistory + " | " : "")
                 + (triageWebhook.fallRisk.HasValue ? "Risco de queda: " + (triageWebhook.fallRisk.Value ? "Sim" : "Não") + " | " : "")
                 + (triageWebhook.idSuspicion.HasValue ? "Suspeita: " + (Util.GetSuspicion(triageWebhook.idSuspicion.Value)) + " | " : "")
                 + (triageWebhook.idPain.HasValue && triageWebhook.idPain.Value >= 1 && triageWebhook.idPain.Value <= 9 ? "Dor: " + (Util.GetPainDescription(triageWebhook.idPain.Value)) + " | " : "")
                + (string.IsNullOrEmpty(triageWebhook.observations) ? "" : "Observações: " + triageWebhook.observations);
            dsSenha = triageWebhook.ticketName;
            cdUsuario = usuarios.cdUsuario;
            nmUsuarioTriagem = usuarios.nmUsuario;
            vlEscore = triageWebhook.score.HasValue ? triageWebhook.score.Value : 0;//Confirmar esse
            tpClassificacao = "COMPLETA";
            cdFilaSenha = variables.getVariable<decimal?>(VariableTypeEnum.fila_senha, triageWebhook.idForward.Value);
            snPrioridadeOcto = vlIdade >= 80 ? "S" : "N";
            snPrioridadeClassificacao = triageWebhook.listIdPriority?.Any() ?? false ? "S" : "N";
            snPrioridadeEspecial = triageWebhook.listIdPriority?.Any() ?? false ? "S" : "N";
            snAtendimentoSocial = "N";
            snConfirmaChamada = "N";
            tpRastreamento = "AGUARDANDO";
            dhChamadaClassificacao = triageWebhook.startClassification.Value.AddHours(3);
            qtChamadas = 1;
        }

        [Key]
        [Column("CD_TRIAGEM_ATENDIMENTO")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("DH_PRE_ATENDIMENTO")]
        public DateTime dhPreAtendimento { get; set; }

        [Column("CD_PACIENTE")]
        public decimal? cdPaciente { get; set; }

        [Column("NM_PACIENTE")]
        public string? nmPaciente { get; set; }

        [Column("DT_NASCIMENTO")]
        public DateTime? dtNascimento { get; set; }

        [Column("TP_SEXO")]
        public char? tpSexo { get; set; }

        [Column("CD_COR_REFERENCIA")]
        public decimal? cdCorReferencia { get; set; }

        [Column("CD_SINTOMA_AVALIACAO")]
        public decimal? cdSintomaAvaliacao { get; set; }

        [Column("CD_CLASSIFICACAO")]
        public decimal? cdClassificacao { get; set; }

        [Column("CD_MULTI_EMPRESA")]
        public decimal cdMultiEmpresa { get; set; }

        [Column("VL_IDADE")]
        public int? vlIdade { get; set; }

        [Column("DH_PRE_ATENDIMENTO_FIM")]
        public DateTime? dhPreAtendimentoFim { get; set; }

        [Column("DS_QUEIXA_PRINCIPAL")]
        public string? dsQueixaPrincipal { get; set; }

        [Column("CD_ESPECIALID")]
        public decimal? cdEspecialid { get; set; }

        [Column("DS_ALERGIA")]
        public string? dsAlergia { get; set; }

        [Column("DS_OBSERVACAO")]
        public string? dsObservacao { get; set; }

        [Column("DS_SENHA")]
        public string? dsSenha { get; set; }

        [Column("CD_USUARIO")]
        public string? cdUsuario { get; set; }

        [Column("NM_USUARIO_TRIAGEM")]
        public string? nmUsuarioTriagem { get; set; }

        [Column("VL_ESCORE")]
        public decimal? vlEscore { get; set; }

        [Column("TP_CLASSIFICACAO")]
        public string? tpClassificacao { get; set; }

        [Column("CD_FILA_SENHA")]
        public decimal? cdFilaSenha { get; set; }

        [Column("SN_PRIORIDADE_OCTO")]
        public string? snPrioridadeOcto { get; set; }

        [Column("SN_PRIORIDADE_CLASSIFICACAO")]
        public string? snPrioridadeClassificacao { get; set; }

        [Column("SN_PRIORIDADE_ESPECIAL")]
        public string? snPrioridadeEspecial { get; set; }

        [Column("SN_ATENDIMENTO_SOCIAL")]
        public string? snAtendimentoSocial { get; set; }

        [Column("SN_CONFIRMA_CHAMADA")]
        public string? snConfirmaChamada { get; set; }

        [Column("TP_RASTREAMENTO")]
        public string tpRastreamento { get; set; }

        [Column("DH_CHAMADA_CLASSIFICACAO")]
        public DateTime? dhChamadaClassificacao { get; set; }

        [Column("QT_CHAMADAS")]
        public decimal? qtChamadas { get; set; }

        //nr_cpf
        //nr_cns
        //cd_paciente
    }
}
