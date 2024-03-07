using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV
{
    public interface IPostgreMVRepository
    {
        public List<RelationEpisode> getLastTicket();
        public RelationEpisode CreateRelation(RelationEpisode relationEpisode);
        public void UpdateRelation(RelationEpisode relationEpisode);
        public void CreateRelation(RelationTriage triage);
        public List<RelationTriage> ReadTriage(long idRelationEpisode);
        public RelationEpisode? GetRelation(long IdEpisode);
        public ListVariableStruct GetRelationConfig();
        public void DeleteConfig();
        public void UpdateConfig(List<IntegrationVariable> integrationVariables);
    }
}
