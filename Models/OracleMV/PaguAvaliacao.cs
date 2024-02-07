using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("PAGU_AVALIACAO")]
    public class PaguAvaliacao
    {
        public PaguAvaliacao() { }
        public PaguAvaliacao(TriageWebhookStruct triageWebhook, decimal cdTriagemAtendimento, ListVariableStruct variables)
        {
            cdFormula = 129;//de-para?
            dhAvaliacao = triageWebhook.startClassification.Value;
            nmUsuario = triageWebhook.userClassificationName;
            vlResultado = 0;//triageWebhook.idPain.ToString();//de-para?
            snExibirResultado = "S";
            this.cdTriagemAtendimento = cdTriagemAtendimento;
            cdMultiEmpresa = variables.getVariable<decimal>(VariableTypeEnum.multi_empresa);
        }

        [Key, Column("cd_avaliacao")]
        public decimal cdAvaliacao { get; set; }

        [Column("cd_formula")]
        public decimal cdFormula { get; set; }

        [Column("dh_avaliacao")]
        public DateTime dhAvaliacao { get; set; }

        [Column("nm_usuario")]
        public string nmUsuario { get; set; }

        [Column("vl_resultado")]
        public float vlResultado { get; set; }

        [Column("sn_exibir_resultado")]
        public string snExibirResultado { get; set; }

        [Column("cd_triagem_atendimento")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("cd_multi_empresa")]
        public decimal cdMultiEmpresa { get; set; }
    }
}
