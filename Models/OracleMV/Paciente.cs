using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("PACIENTE")]
    public class Paciente
    {
        [Key]
        [Column("CD_PACIENTE")]
        public decimal cdPaciente { get; set; }

        [Column("NR_CPF")]
        public string? nrCpf { get; set; }

        [Column("NM_PACIENTE")]
        public string nmPaciente { get; set; }

        [Column("NR_CNS")]
        public string? nrCns { get; set; }

        [Column("CD_MULTI_EMPRESA")]
        public decimal cdMultiEmpresa { get; set; }

        [Column("TP_SEXO")]
        public char tpSexo { get; set; }

        [Column("DT_NASCIMENTO")]
        public DateTime? dtNascimento { get; set; }

        [Column("NM_MAE")]
        public string? nmMae { get; set; }
    }
}
