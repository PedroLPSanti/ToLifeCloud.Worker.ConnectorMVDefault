using Microsoft.EntityFrameworkCore;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    public class OracleDBASGUContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("DBASGU");
        }

        public OracleDBASGUContext(DbContextOptions<OracleDBASGUContext> options) : base(options)
        {

        }

        public DbSet<Usuarios> usuarios { get; set; }
    }
}
