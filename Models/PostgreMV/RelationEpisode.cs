﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV
{
    [Table("relation_episode")]
    public class RelationEpisode
    {
        public RelationEpisode(long idEpisode, decimal cdTriagemAtendimento)
        {
            this.idEpisode = idEpisode;
            this.cdTriagemAtendimento = cdTriagemAtendimento;
            datetimeInclusion = DateTime.UtcNow;
        }

        [Key]
        [Column("id_relation_episode")]
        public long idRelationEpisode { get; set; }

        [Column("id_episode")]
        public long idEpisode { get; set; }

        [Column("cd_triagem_atendimento")]
        public decimal cdTriagemAtendimento { get; set; }

        [Column("datetime_inclusion")]
        public DateTime datetimeInclusion { get; set; }
    }
}
