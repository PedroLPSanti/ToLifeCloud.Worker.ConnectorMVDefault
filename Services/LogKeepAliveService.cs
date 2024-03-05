using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ToLifeCloud.SRVLog;
using ToLifeCloud.SRVLog.Enum;
using ToLifeCloud.SRVLog.Response;
using ToLifeCloud.Worker.ConnectorMVDefault.Client;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Requests;
using ToLifeCloud.Worker.ConnectorMVDefault.Responses;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public class LogKeepAliveService
    {
        private AppSettings _appSettings;
        private Log _log;

        //private BillingDataRepository _billingDataRepository;

        private IServiceProvider Services { get; }
        public LogKeepAliveService(AppSettings appSettings, IServiceProvider services)
        {
            Services = services;
            _appSettings = appSettings;
            _log = new Log(_appSettings.urlApiSrvLog);
            Services = services;
        }

        public void Process()
        {
            try
            {
                using (var scope = Services.CreateScope())
                {
                    IPostgreMVRepository postgreRepository = scope.ServiceProvider.GetRequiredService<IPostgreMVRepository>();

                    ListVariableStruct variables = postgreRepository.GetRelationConfig();

                    if (!variables.hasValues()) return;

                    IntegrationRelationConfig.SendKeepAlive(_appSettings.urlIntegrationRelationConfig,
                        new KeepAliveRequest(variables.variables.First().idHealthUnitRelation),
                        variables.getVariable<string>(VariableTypeEnum.token));
                }
            }
            catch (Exception ex)
            {

                _log.Save(LogTypeEnum.Error, (int)_appSettings.idHealthUnit,
                    $"{MethodBase.GetCurrentMethod().ReflectedType.Name}.{MethodBase.GetCurrentMethod().Name}",
                    null, null, ex);
            }
        }
    }
}
