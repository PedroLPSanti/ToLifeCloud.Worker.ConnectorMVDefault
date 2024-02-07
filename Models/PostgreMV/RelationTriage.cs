using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV
{
    [Table("relation_triage")]
    public class RelationTriage
    {
        public RelationTriage(long idRelationEpisode, long idTriage, decimal cdColetaSinalVital)
        {
            this.idRelationEpisode = idRelationEpisode;
            this.idTriage = idTriage;
            this.cdColetaSinalVital = cdColetaSinalVital;
            datetimeInclusion = DateTime.UtcNow;
        }

        [Key]
        [Column("id_relation_triage")]
        public long idRelationTriage { get; set; }

        [Column("id_relation_episode")]
        public long idRelationEpisode { get; set; }

        [Column("id_triage")]
        public long idTriage { get; set; }

        [Column("cd_coleta_sinal_vital")]
        public decimal cdColetaSinalVital { get; set; }

        [Column("datetime_inclusion")]
        public DateTime datetimeInclusion { get; set; }
    }
}
