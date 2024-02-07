using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    public class SinalVitalStruct: PwSinalVitalUnidInstrAfer
    {
        public decimal cdSinalVital { get; set; }

        public string dsSinalVital { get; set; }
    }
}
