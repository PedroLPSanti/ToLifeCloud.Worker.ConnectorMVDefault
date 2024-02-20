using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("COLETA_SINAL_VITAL")]
    public class ColetaSinalVital
    {
        public ColetaSinalVital()
        { }
        public ColetaSinalVital(TriageWebhookStruct triage, Usuarios usuarios, decimal cdTriagemAtendimento, ListVariableStruct variables)
        {
            cdPrestador = usuarios.cdPrestador;
            nmUsuario = triage.userClassificationName;
            dataColeta = triage.startClassification.Value.AddHours(3);
            this.cdTriagemAtendimento = cdTriagemAtendimento;
            snFinalizado = 'N';
            cdMultiEmpresa = variables.getVariable<decimal>(VariableTypeEnum.multi_empresa);//de-para
            snRelevante = 'S';
        }

        [Key]
        [Column("CD_COLETA_SINAL_VITAL")]
        public decimal cdColetaSinalVital { get; set; }

        [Column("CD_PRESTADOR")]
        public decimal cdPrestador { get; set; }

        [Column("NM_USUARIO")]
        public string nmUsuario { get; set; }

        [Column("DATA_COLETA")]
        public DateTime dataColeta { get; set; }

        [Column("CD_TRIAGEM_ATENDIMENTO")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("SN_FINALIZADO")]
        public char snFinalizado { get; set; }

        [Column("CD_MULTI_EMPRESA")]
        public decimal cdMultiEmpresa { get; set; }

        [Column("SN_RELEVANTE")]
        public char snRelevante { get; set; }

    }
}
