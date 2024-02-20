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
        public PaguAvaliacao(TriageWebhookStruct triageWebhook, decimal cdTriagemAtendimento, ListVariableStruct variables, VariableTypeEnum valueType, float vlResultado)
        {
            cdFormula = variables.getVariable<decimal>(valueType);//de-para?
            dhAvaliacao = triageWebhook.startClassification.Value.AddHours(3);
            nmUsuario = triageWebhook.userClassificationName;
            this.vlResultado = vlResultado;
            snExibirResultado = "S";
            this.cdTriagemAtendimento = cdTriagemAtendimento;
            cdMultiEmpresa = variables.getVariable<decimal>(VariableTypeEnum.multi_empresa);
        }

        [Key, Column("CD_AVALIACAO")]
        public decimal cdAvaliacao { get; set; }

        [Column("CD_FORMULA")]
        public decimal cdFormula { get; set; }

        [Column("DH_AVALIACAO")]
        public DateTime dhAvaliacao { get; set; }

        [Column("NM_USUARIO")]
        public string nmUsuario { get; set; }

        [Column("VL_RESULTADO")]
        public float vlResultado { get; set; }

        [Column("SN_EXIBIR_RESULTADO")]
        public string snExibirResultado { get; set; }

        [Column("CD_TRIAGEM_ATENDIMENTO")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("CD_MULTI_EMPRESA")]
        public decimal cdMultiEmpresa { get; set; }
    }
}
