using Microsoft.EntityFrameworkCore;
using System.Linq;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV
{
    public class PostgreMVRepository : IPostgreMVRepository
    {
        private PostgreMVContext _context;
        public PostgreMVRepository(PostgreMVContext context)
        {
            _context = context;
        }

        public List<RelationEpisode> getLastTicket()
        {
            var todayLimits = DateTime.UtcNow.AddHours(-5);

            var atendimentos = _context.relationEpisode.Where(c => c.isMv && !c.cdAtendimento.HasValue && c.datetimeInclusion >= todayLimits).OrderBy(c => c.datetimeInclusion).ToList();

            var last = _context.relationEpisode.Where(c => c.isMv && c.cdAtendimento.HasValue && c.datetimeInclusion >= todayLimits).OrderByDescending(c => c.datetimeInclusion).FirstOrDefault();

            if (!(atendimentos?.Any() ?? false) && last == null) return new List<RelationEpisode>();

            if (!(atendimentos?.Any() ?? false)) return new List<RelationEpisode> { last };

            if (last != null && !atendimentos.Any(c => c.cdTriagemAtendimento > last.cdTriagemAtendimento)) atendimentos.Add(last);

            return atendimentos;
        }

        public RelationEpisode CreateRelation(RelationEpisode relationEpisode)
        {
            _context.relationEpisode.Add(relationEpisode);
            _context.SaveChanges();
            return relationEpisode;
        }

        public void CreateRelation(RelationTriage triage)
        {
            _context.relationTriage.Add(triage);
            _context.SaveChanges();
        }

        public void UpdateRelation(RelationEpisode relationEpisode)
        {
            var relation = _context.relationEpisode.Where(c => c.cdTriagemAtendimento == relationEpisode.cdTriagemAtendimento).FirstOrDefault();
            relation.idEpisode = relationEpisode.idEpisode;
            relation.cdAtendimento = relationEpisode.cdAtendimento;
            _context.relationEpisode.Update(relation);
            _context.SaveChanges();
        }

        public List<RelationTriage> ReadTriage(long idRelationEpisode)
        {
            return _context.relationTriage.Where(c => c.idRelationEpisode == idRelationEpisode).ToList();
        }

        public RelationEpisode? GetRelation(long IdEpisode)
        {
            return _context.relationEpisode.Where(c => c.idEpisode == IdEpisode).FirstOrDefault();
        }

        public ListVariableStruct GetRelationConfig()
        {
            ListVariableStruct list = new ListVariableStruct();
            list.variables = _context.integrationVariable.ToList();
            return list;
        }

        public void DeleteConfig()
        {
            var result = _context.integrationVariable.AsQueryable();
            _context.integrationVariable.RemoveRange(result);
            _context.SaveChanges();
        }

        public void UpdateConfig(List<IntegrationVariable> integrationVariables)
        {

            _context.Database.CreateExecutionStrategy().Execute(
                    () =>
                    {
                        using (var dbContextTransaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var result = _context.integrationVariable.AsQueryable();
                                _context.integrationVariable.RemoveRange(result);
                                _context.SaveChanges();
                                _context.integrationVariable.AddRange(integrationVariables);
                                _context.SaveChanges();
                                dbContextTransaction.Commit();
                            }
                            catch (Exception)
                            {
                                dbContextTransaction.Rollback();
                                throw;
                            }
                        }
                    });
        }
    }
}
