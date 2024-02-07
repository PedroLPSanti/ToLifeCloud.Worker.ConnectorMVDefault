using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Responses
{
    public class IntegrationVariableResponse : ReturnStruct
    {
        public IntegrationVariableResponse() {
            integrationVariables = new List<IntegrationVariable>();
        }
        public List<IntegrationVariable> integrationVariables { get; set; }
    }
}
