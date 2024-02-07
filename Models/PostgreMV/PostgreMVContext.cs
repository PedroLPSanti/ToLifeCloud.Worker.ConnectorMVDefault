using Microsoft.EntityFrameworkCore;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV
{
    public class PostgreMVContext : DbContext
    {
        public PostgreMVContext(DbContextOptions<PostgreMVContext> options) : base(options)
        {

        }

        public DbSet<RelationEpisode> relationEpisode { get; set; }

        public DbSet<IntegrationVariable> integrationVariable { get; set; }

        public DbSet<RelationTriage> relationTriage { get; set; }
    }
}
