using Microsoft.EntityFrameworkCore;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV
{
    public class OracleDBAMVContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder.HasDefaultSchema("DBAMV")
                .Entity<ItColetaSinalVital>(c => c.HasKey(o => new { o.cdColetaSinalVital, o.cdSinalVital })));
        }

        public OracleDBAMVContext(DbContextOptions<OracleDBAMVContext> options) : base(options)
        {

        }

        public DbSet<ColetaSinalVital> coletaSinalVital { get; set; }

        public DbSet<ItColetaSinalVital> itColetaSinalVital { get; set; }

        public DbSet<Paciente> paciente { get; set; }

        public DbSet<Prestador> prestador { get; set; }

        public DbSet<PwSinalVitalUnidInstrAfer> pwSinalVitalUnidInstrAfer { get; set; }

        public DbSet<SacrClassificacaoRisco> sacrClassificacaoRisco { get; set; }

        public DbSet<SacrProtocolo> sacrProtocolo { get; set; }

        public DbSet<SacrSenhaTriagem> sacrSenhaTriagem { get; set; }

        public DbSet<SinalVital> sinalVital { get; set; }

        public DbSet<TriagemAtendimento> triagemAtendimento { get; set; }

        public DbSet<PaguAvaliacao> paguAvaliacao { get; set; }
    }
}
