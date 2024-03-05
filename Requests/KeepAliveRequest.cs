namespace ToLifeCloud.Worker.ConnectorMVDefault.Requests
{
    public class KeepAliveRequest
    {
        public KeepAliveRequest() { }
        public KeepAliveRequest(long idHealthUnitRelation)
        {
            this.idHealthUnitRelation = idHealthUnitRelation;
        }
        public long idHealthUnitRelation { get; set; }
    }
}
