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
    public class TicketEvasionService
    {
        private AppSettings _appSettings;
        private Log _log;
        private LogBatch _logBatch;

        private IServiceProvider Services { get; }
        public TicketEvasionService(AppSettings appSettings, IServiceProvider services)
        {
            Services = services;
            _appSettings = appSettings;
            _logBatch = new LogBatch(_appSettings.urlApiSrvLog);
            _log = new Log(_appSettings.urlApiSrvLog);
            Services = services;
        }

        public void Process()
        {
            EvasionWebhookStruct? evasion = null;
            try
            {
                using (var scope = Services.CreateScope())
                {
                    var oracleMVRepository = scope.ServiceProvider.GetRequiredService<IOracleMVRepository>();
                    var postgreRepository = scope.ServiceProvider.GetRequiredService<IPostgreMVRepository>();

                    var variables = postgreRepository.GetRelationConfig();

                    if (!variables.hasValues()) return;

                    var hash = variables.getVariable<string>(VariableTypeEnum.token);

                    evasion = SRVMessageQueue.GetMessage<EvasionWebhookStruct?>(_appSettings.urlSRVMessageQueue, "TicketEvasion", hash);

                    if (evasion == null) return;

                    RelationEpisode relationEpisode = postgreRepository.GetRelation(evasion.idEpisode);

                    if (relationEpisode == null) return;

                    oracleMVRepository.DeleteAtendimentoTriagem(relationEpisode.cdTriagemAtendimento);
                }
            }
            catch (Exception ex)
            {

                _log.Save(LogTypeEnum.Error, (int)_appSettings.idHealthUnit,
                    $"{MethodBase.GetCurrentMethod().ReflectedType.Name}.{MethodBase.GetCurrentMethod().Name}",
                    null, $"{JsonConvert.SerializeObject(evasion)}", ex);
            }
        }
    }
}
