using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV
{
    [Table("integration_variable")]
    public class IntegrationVariable
    {
        [Key]
        [Column("id_integration_variable")]
        public long idIntegrationVariable { get; set; }
        [Column("id_variable_type")]
        public long idVariableType { get; set; }
        [Column("variable_to_life")]
        public string variableToLife { get; set; }
        [Column("variable_integration")]
        public string variableIntegration { get; set; }
        [Column("id_health_unit_relation")]
        public long idHealthUnitRelation { get; set; }
    }
}
