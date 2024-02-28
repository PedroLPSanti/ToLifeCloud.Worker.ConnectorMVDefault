using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Structs
{
    public class ListVariableStruct
    {
        public ListVariableStruct()
        {
            variables = new List<IntegrationVariable>();
        }
        public List<IntegrationVariable> variables { get; set; }

        public bool hasValues()
        {
            return variables?.Any() ?? false;
        }


        public IntegrationVariable? getVariableNullable(VariableTypeEnum variableType)
        {
            var result = variables.Where(c => c.idVariableType == (int)variableType).FirstOrDefault();
            return result;
        }

        public T getVariable<T>(VariableTypeEnum variableType)
        {
            var result = variables.Where(c => c.idVariableType == (int)variableType).FirstOrDefault();
            if (result == null)
            {
                throw new InvalidOperationException($"Variável de integração não configurada: {variableType}.");
            }
            return (T)Convert.ChangeType(result.variableIntegration, typeof(T));
        }

        public List<IntegrationVariable> getListVariable(VariableTypeEnum variableType)
        {
            var result = variables.Where(c => c.idVariableType == (int)variableType).ToList();
            if (!(result?.Any() ?? false))
            {
                throw new InvalidOperationException($"Variável de integração não configurada: {variableType}.");
            }
            return result;
        }

        public T getVariable<T>(VariableTypeEnum variableType, string variableToLife)
        {
            var result = variables.Where(c => c.idVariableType == (int)variableType && c.variableToLife == variableToLife).FirstOrDefault();
            if (result == null)
            {
                throw new InvalidOperationException($"Variável de integração não configurada: {variableType}.");
            }
            return (T)Convert.ChangeType(result.variableIntegration, typeof(T));
        }

        public T getVariable<T>(VariableTypeEnum variableType, int variableToLife)
        {
            return getVariable<T>(variableType, variableToLife.ToString());
        }

        public T getVariable<T>(VariableTypeEnum variableType, long variableToLife)
        {
            return getVariable<T>(variableType, variableToLife.ToString());
        }

        public T getVariableTolife<T>(VariableTypeEnum variableType, string variableIntegration)
        {
            var result = variables.Where(c => c.idVariableType == (int)variableType && c.variableIntegration == variableIntegration).FirstOrDefault();
            if (result == null)
            {
                throw new InvalidOperationException($"Variável de integração ToLife não configurada: {variableType}.");
            }
            return (T)Convert.ChangeType(result.variableToLife, typeof(T));
        }
    }
}
