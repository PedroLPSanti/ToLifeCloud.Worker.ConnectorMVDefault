using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    public class UsuariosStruct : Prestador
    {
        public string cdUsuario { get; set; }
        
        public string nmUsuario { get; set; }
        
        public string tpPrivilegio { get; set; }
        
        public decimal cdPrestador { get; set; }
    }
}
