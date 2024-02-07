using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    [Table("USUARIOS")]
    public class Usuarios
    {
        [Key, Column("CD_USUARIO")]
        public string cdUsuario { get; set; }

        [Column("CPF")]
        public string cpf { get; set; }

        [Column("CD_PRESTADOR")]
        public decimal cdPrestador { get; set; }
    }
}
