using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToLifeCloud.SRVLog;
using ToLifeCloud.SRVLog.Enum;
using ToLifeCloud.SRVLog.Response;
using ToLifeCloud.Worker.ConnectorMVDefault.Client;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public class PatientTriageService
    {
        private AppSettings _appSettings;
        private Log _log;
        private LogBatch _logBatch;
        private IServiceProvider Services { get; }
        //private BillingDataRepository _billingDataRepository;

        public PatientTriageService(AppSettings appSettings, IServiceProvider services)
        {
            _appSettings = appSettings;
            _log = new Log(_appSettings.urlApiSrvLog);
            Services = services;
            //_billingDataRepository = new BillingDataRepository(_appSettings);
        }

        public void Process()
        {
            WebhookRequestStruct? webhookRequest = null;

            try
            {
                using (var scope = Services.CreateScope())
                {
                    var oracleMVRepository = scope.ServiceProvider.GetRequiredService<IOracleMVRepository>();
                    var postgreRepository = scope.ServiceProvider.GetRequiredService<IPostgreMVRepository>();
                    ListVariableStruct variables = postgreRepository.GetRelationConfig();

                    if (!variables.hasValues()) return;

                    var hash = variables.getVariable<string>(VariableTypeEnum.token);

                    webhookRequest = SRVMessageQueue.GetMessage<WebhookRequestStruct>(_appSettings.urlSRVMessageQueue, "PatientTriage", hash);

                    if (webhookRequest == null) return;

                    RelationEpisode episodeRelation = postgreRepository.GetRelation(webhookRequest.body.idEpisode);

                    var user = oracleMVRepository.GetUserByCpf(webhookRequest.body.userClassificationCPF);

                    if (user == null)
                    {
                        throw new Exception($"CPF do user {webhookRequest.body.userClassificationName}, não cadastrado!");
                    }

                    oracleMVRepository.ValidateGravityConfig(variables, webhookRequest.body.idGravity);

                    Paciente? paciente = null;

                    if (!string.IsNullOrEmpty(webhookRequest.body.patientCpf) || !string.IsNullOrEmpty(webhookRequest.body.patientCns))
                    {
                        paciente = oracleMVRepository.GetPacienteByCpf(variables.getVariable<decimal>(VariableTypeEnum.multi_empresa),
                            webhookRequest.body.patientCpf,
                            webhookRequest.body.patientCns);
                    }

                    TriagemAtendimento triagemAtendimento = new(webhookRequest.body, user, variables, paciente);

                    bool isReclassification = false;

                    if (episodeRelation == null)
                    {
                        var cdTriagemAtendimento = oracleMVRepository.InsertTriage(triagemAtendimento);

                        string dsSenha = oracleMVRepository.GetTriagemAtendimento(cdTriagemAtendimento);

                        oracleMVRepository.UpdateSenhaTriagem(variables, webhookRequest.body, dsSenha);

                        episodeRelation = postgreRepository.CreateRelation(new RelationEpisode(webhookRequest.body.idEpisode, triagemAtendimento.cdTriagemAtendimento));

                        triagemAtendimento.dsSenha = webhookRequest.body.ticketName;
                    }
                    else
                    {
                        triagemAtendimento.cdTriagemAtendimento = episodeRelation.cdTriagemAtendimento;

                        var triageRelation = postgreRepository.ReadTriage(episodeRelation.idRelationEpisode);

                        if (triageRelation?.Any() ?? false) isReclassification = true;
                    }

                    SacrTempoProcesso sacrTempoProcesso = new SacrTempoProcesso(triagemAtendimento.cdTriagemAtendimento, variables.getVariable<decimal>(VariableTypeEnum.tipo_processo_inicio_classificação), user.nmUsuario, webhookRequest.body.startClassification.Value.AddHours(3));

                    oracleMVRepository.InsertTempoProcesso(sacrTempoProcesso);

                    oracleMVRepository.UpdateTriage(triagemAtendimento);

                    ColetaSinalVital coletaSinal = oracleMVRepository.InsertColetaSinalVital(new ColetaSinalVital(webhookRequest.body, user, triagemAtendimento.cdTriagemAtendimento, variables));

                    List<decimal> listSinalVital = new List<decimal>();

                    if (webhookRequest.body.hasVitalSigns())
                    {
                        if (webhookRequest.body.weight.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_peso, variables, coletaSinal, webhookRequest.body.weight.Value));

                        if (webhookRequest.body.height.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_altura, variables, coletaSinal, webhookRequest.body.height.Value));

                        if (webhookRequest.body.temperature.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_temperatura, variables, coletaSinal, webhookRequest.body.temperature.Value));

                        if (webhookRequest.body.heartRate.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_pulso, variables, coletaSinal, webhookRequest.body.heartRate.Value));

                        if (webhookRequest.body.bloodPressureSystole.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_pressao_sistolica, variables, coletaSinal, webhookRequest.body.bloodPressureSystole.Value));

                        if (webhookRequest.body.bloodPressureDiastole.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_pressao_diastolica, variables, coletaSinal, webhookRequest.body.bloodPressureDiastole.Value));

                        if (webhookRequest.body.respiratoryFrequency.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_frequencia_respiratoria, variables, coletaSinal, webhookRequest.body.respiratoryFrequency.Value));

                        if (webhookRequest.body.saturation.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_oxigenio, variables, coletaSinal, webhookRequest.body.saturation.Value));

                        if (webhookRequest.body.idPain.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_dor, variables, coletaSinal, Util.GetPain(webhookRequest.body.idPain.Value)));

                        if (webhookRequest.body.glucose.HasValue)
                            listSinalVital.Add(SaveItColetaSinalVital(oracleMVRepository, VariableTypeEnum.sinal_vital_glicose, variables, coletaSinal, webhookRequest.body.glucose.Value));
                    }
                    decimal? cdAvaliacao = null;
                    if (webhookRequest.body.glasgow.HasValue)
                        cdAvaliacao = oracleMVRepository.InsertPaguAvaliacao(new PaguAvaliacao(webhookRequest.body, triagemAtendimento.cdTriagemAtendimento, variables, VariableTypeEnum.sinal_vital_glasgow, webhookRequest.body.glasgow.Value));


                    oracleMVRepository.InsertClassificacaoRisco(new SacrClassificacaoRisco(triagemAtendimento));

                    triagemAtendimento.tpClassificacao = "COMPLETA";

                    oracleMVRepository.UpdateTriage(triagemAtendimento);

                    sacrTempoProcesso.cdTipoTempoProcesso = variables.getVariable<decimal>(VariableTypeEnum.tipo_processo_fim_classificação);

                    sacrTempoProcesso.dhProcesso = triagemAtendimento.dhPreAtendimentoFim.Value;

                    oracleMVRepository.InsertTempoProcesso(sacrTempoProcesso);

                    oracleMVRepository.CompleteColetaSinalVital(coletaSinal.cdColetaSinalVital);

                    var cdTriagemHist = oracleMVRepository.InsertTriagemAtendimentoHist(new TriagemAtendimentoHist(triagemAtendimento, isReclassification, variables, webhookRequest.body));

                    if (listSinalVital?.Any() ?? false)
                        listSinalVital.ForEach((c) =>
                        {
                            oracleMVRepository.InsertTriaAtndHisItColSinVit(cdTriagemHist, coletaSinal.cdColetaSinalVital, c);
                        });

                    if (webhookRequest.body.glasgow.HasValue && cdAvaliacao.HasValue)
                        oracleMVRepository.InsertTriagAtendimeHistPaguAval(cdTriagemHist, cdAvaliacao.Value);

                    postgreRepository.CreateRelation(new RelationTriage(episodeRelation.idRelationEpisode, webhookRequest.body.idTriage, coletaSinal.cdColetaSinalVital));
                }
            }
            catch (Exception ex)
            {
                _log.Save(LogTypeEnum.Error, (int)_appSettings.idHealthUnit,
                    $"{MethodBase.GetCurrentMethod().ReflectedType.Name}.{MethodBase.GetCurrentMethod().Name}",
                    null, JsonConvert.SerializeObject(webhookRequest), ex);
            }
        }

        private decimal SaveItColetaSinalVital(IOracleMVRepository oracleMVRepository, VariableTypeEnum typeSinalVital, ListVariableStruct variables, ColetaSinalVital coletaSinal, float value)
        {
            var sinalVital = oracleMVRepository.GetSinalVital(variables.getVariable<decimal>(typeSinalVital));
            oracleMVRepository.InsertColetaSinalVital(new ItColetaSinalVital(sinalVital, coletaSinal.cdColetaSinalVital, value));
            return sinalVital.cdSinalVital;
        }
    }
}
