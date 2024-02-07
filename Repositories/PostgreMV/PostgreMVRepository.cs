using Microsoft.EntityFrameworkCore;
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

        //public RelationEpisode? getLassTicket()
        //{

        //    return _context.relationEpisode.Where(c => c.dtGeracaoSenha.HasValue).OrderByDescending(c => c.dtGeracaoSenha).FirstOrDefault();
        //}

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
