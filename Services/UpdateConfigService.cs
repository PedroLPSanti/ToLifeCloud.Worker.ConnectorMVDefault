using System.Reflection;
using ToLifeCloud.SRVLog;
using ToLifeCloud.SRVLog.Enum;
using ToLifeCloud.SRVLog.Response;
using ToLifeCloud.Worker.ConnectorMVDefault.Client;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Responses;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public class UpdateConfigService
    {
        private AppSettings _appSettings;
        private Log _log;
        private LogBatch _logBatch;

        //private BillingDataRepository _billingDataRepository;

        private IServiceProvider Services { get; }
        public UpdateConfigService(AppSettings appSettings, IServiceProvider services)
        {
            Services = services;
            _appSettings = appSettings;
            _logBatch = new LogBatch(_appSettings.urlApiSrvLog);
            _log = new Log(_appSettings.urlApiSrvLog);
            Services = services;
            //_billingDataRepository = new BillingDataRepository(_appSettings);
        }

        public void Process()
        {
            LogBatchResponse logBatchResponse = null;
            try
            {
                using (var scope = Services.CreateScope())
                {
                    var postgreRepository = scope.ServiceProvider.GetRequiredService<IPostgreMVRepository>();
                    var variables = postgreRepository.GetRelationConfig();
                    MessageQueueResponse message = new MessageQueueResponse();
                    bool hasHash = false;
                    if (variables.hasValues())
                    {
                        var hash = variables.getVariable<string>(VariableTypeEnum.token);//Tem que ser a hash de integração;
                        if (hash != null && !string.IsNullOrEmpty(hash))
                        {
                            hasHash = true;
                            message = SRVMessageQueue.GetMessage(_appSettings.urlSRVMessageQueue, "UpdateConfig", hash);
                        }
                    }
                    bool value;
                    if (!hasHash || (!message.isError && !string.IsNullOrEmpty(message.message) && Boolean.TryParse(message.message, out value) && value))
                    {
                        var result = IntegrationRelationConfig.GetVariables(_appSettings.urlIntegrationRelationConfig, _appSettings.idHealthUnit, _appSettings.internalLoginHash);
                        if (!result.isError)
                        {
                            if ((result.integrationVariables?.Any() ?? false))
                            {
                                postgreRepository.UpdateConfig(result.integrationVariables);
                            }
                            else
                            {
                                postgreRepository.DeleteConfig();
                            }
                        }
                    }
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
