using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("ITCOLETA_SINAL_VITAL")]
    public class ItColetaSinalVital
    {
        public ItColetaSinalVital()
        {
        }
        public ItColetaSinalVital(SinalVitalStruct sinalVital, decimal cdColetaSinalVital, float valor)
        {
            cdSinalVital = sinalVital.cdSinalVital;
            snAtivo = "S";
            snIncluidoNpadronizado = "N";
            cdInstrumentoAfericao = sinalVital.cdInstrumentoAfericao;
            cdUnidadeAfericao = sinalVital.cdUnidadeAfericao;
            nrOrdem = sinalVital.cdSinalVital;
            tpLancamento = sinalVital.tpLancamento;
            this.cdColetaSinalVital = cdColetaSinalVital;
            this.valor = valor;
        }

        [Column("CD_COLETA_SINAL_VITAL")]
        public decimal cdColetaSinalVital { get; set; }

        [Column("CD_INSTRUMENTO_AFERICAO")]
        public decimal cdInstrumentoAfericao { get; set; }

        [Column("CD_SINAL_VITAL")]
        public decimal cdSinalVital { get; set; }

        [Column("CD_UNIDADE_AFERICAO")]
        public decimal cdUnidadeAfericao { get; set; }

        [Column("NR_ORDEM")]
        public decimal nrOrdem { get; set; }

        [Column("SN_ATIVO")]
        public string snAtivo { get; set; }

        [Column("SN_INCLUIDO_NPADRONIZADO")]
        public string snIncluidoNpadronizado { get; set; }

        [Column("TP_LANCAMENTO")]
        public string tpLancamento { get; set; }

        [Column("VALOR")]
        public float valor { get; set; }
    }
}
