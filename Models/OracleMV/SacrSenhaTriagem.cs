using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("SACR_SENHA_TRIAGEM")]
    public class SacrSenhaTriagem
    {
        [Key, Column("CD_SENHA_TRIAGEM")]
        public decimal cdSenhaTriagem { get; set; }
        
        [Column("CD_SENHA")]
        public decimal? cdSenha { get; set; }
        
        [Column("TP_SENHA")]
        public string? tpSenha { get; set; }

        [Column("DH_SENHA")]
        public DateTime? dhSenha { get; set; }
        
        [Column("CD_SETOR")]
        public decimal? cdSetor { get; set; }
        
        [Column("CD_MULTI_EMPRESA")]
        public decimal? cdMultiEmpresa { get; set; }
        
        [Column("CD_FILA_SENHA")]
        public decimal? cdFilaSenha { get; set; }
        
        [Column("QT_REINICIALIZACAO")]
        public decimal? qtReinicializacao { get; set; }
    }
}
