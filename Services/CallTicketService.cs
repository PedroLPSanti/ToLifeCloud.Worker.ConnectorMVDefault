﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToLifeCloud.SRVLog;
using ToLifeCloud.SRVLog.Enum;
using ToLifeCloud.SRVLog.Response;
using ToLifeCloud.Worker.ConnectorMVDefault.Client;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public class CallTicketService
    {
        private AppSettings _appSettings;
        private Log _log;
        private LogBatch _logBatch;

        //private BillingDataRepository _billingDataRepository;

        private IServiceProvider Services { get; }
        public CallTicketService(AppSettings appSettings, IServiceProvider services)
        {
            Services = services;
            _appSettings = appSettings;
            _logBatch = new LogBatch(_appSettings.urlApiSrvLog);
            _log = new Log(_appSettings.urlApiSrvLog);
            Services = services;
        }

        public void Process()
        {
            LogBatchResponse logBatchResponse = null;
            string? objectMessage = null;
            try
            {
                using (var scope = Services.CreateScope())
                {
                    var oracleMVRepository = scope.ServiceProvider.GetRequiredService<IOracleMVRepository>();
                    var postgreRepository = scope.ServiceProvider.GetRequiredService<IPostgreMVRepository>();
                    var variables = postgreRepository.GetRelationConfig();
                    if (variables.hasValues())
                    {
                        var hash = variables.getVariable<string>(VariableTypeEnum.token);
                        var message = SRVMessageQueue.GetMessage(_appSettings.urlSRVMessageQueue, "CallTicket", hash);
                        if (message.isError)
                        {
                            throw new Exception(message.errorDescription);
                        }
                        else if (string.IsNullOrEmpty(message.message))
                        {
                            return;
                        }
                        objectMessage = message.message;
                        PanelCallWebhookStruct panelCall = JsonConvert.DeserializeObject<PanelCallWebhookStruct>(message.message);

                        RelationEpisode relationEpisode = postgreRepository.GetRelation(panelCall.idEpisode);
                        if (relationEpisode == null) return;
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
