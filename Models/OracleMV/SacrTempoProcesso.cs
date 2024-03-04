using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_TEMPO_PROCESSO")]
    public class SacrTempoProcesso
    {
        public SacrTempoProcesso() { }

        public SacrTempoProcesso(decimal cdTriagemAtendimento, decimal cdTipoTempoProcesso, string nmUsuario, DateTime dhProcesso)
        {
            this.cdTriagemAtendimento = cdTriagemAtendimento;
            this.cdTipoTempoProcesso = cdTipoTempoProcesso;
            this.dhProcesso = dhProcesso;
            nmMaquina = "Integração ToLife";
            this.nmUsuario = nmUsuario;
            dsSequenciaProcesso = "1,2,3,4,5";
        }
        [Key, Column("CD_TEMPO_PROCESSO")]
        public decimal cdTempoProcesso { get; set; }
        [Column("CD_TIPO_TEMPO_PROCESSO")]
        public decimal cdTipoTempoProcesso { get; set; }
        [Column("CD_TRIAGEM_ATENDIMENTO")]
        public decimal cdTriagemAtendimento { get; set; }
        [Column("DH_PROCESSO")]
        public DateTime dhProcesso { get; set; }
        [Column("NM_MAQUINA")]
        public string nmMaquina { get; set; }
        [Column("NM_USUARIO")]
        public string nmUsuario { get; set; }
        [Column("DS_SEQUENCIA_PROCESSO")]
        public string dsSequenciaProcesso { get; set; }
    }
}
