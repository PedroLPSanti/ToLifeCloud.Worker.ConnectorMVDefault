using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_TEMPO_PROCESSO")]
    public class SacrTempoProcesso
    {
        //SELECT * FROM SACR_TEMPO_PROCESSO
        //SELECT* FROM SACR_TIPO_TEMPO_PROCESSO

        public SacrTempoProcesso() { }

        public SacrTempoProcesso(decimal cdTriagemAtendimento, decimal cdTipoTempoProcesso, string nmUsuario)
        {
            this.cdTriagemAtendimento = cdTriagemAtendimento;
            this.cdTipoTempoProcesso = cdTipoTempoProcesso;
            dhProcesso = DateTime.Now;
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
