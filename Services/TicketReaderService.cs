using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToLifeCloud.SRVLog;
using ToLifeCloud.SRVLog.Enum;
using ToLifeCloud.SRVLog.Response;
using ToLifeCloud.Worker.ConnectorMVDefault.Client;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Requests;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;

namespace ToLifeCloud.Worker.ConnectorMVDefault
{
    public class TicketReaderService
    {
        private AppSettings _appSettings;
        private Log _log;
        private LogBatch _logBatch;

        //private BillingDataRepository _billingDataRepository;

        private IServiceProvider Services { get; }
        public TicketReaderService(AppSettings appSettings, IServiceProvider services)
        {
            Services = services;
            _appSettings = appSettings;
            _logBatch = new LogBatch(_appSettings.urlApiSrvLog);
            _log = new Log(_appSettings.urlApiSrvLog);
            Services = services;
            //_billingDataRepository = new BillingDataRepository(_appSettings);
        }

        public bool Process()
        {
            LogBatchResponse logBatchResponse = null;
            try
            {
                using (var scope = Services.CreateScope())
                {
                    IOracleMVRepository oracleRepository = scope.ServiceProvider.GetRequiredService<IOracleMVRepository>();
                    IPostgreMVRepository postgreRepository = scope.ServiceProvider.GetRequiredService<IPostgreMVRepository>();
                    ListVariableStruct variables = postgreRepository.GetRelationConfig();

                    if (!variables.hasValues()) return true;

                    var ListLastTicket = postgreRepository.getLastTicket();

                    var listIncomplete = ListLastTicket.Where(x => !x.cdAtendimento.HasValue).Select(c => c.cdTriagemAtendimento).ToList();

                    TriagemAtendimento? ticket = null;
                    var isNew = true;
                    if (listIncomplete?.Any() ?? false)
                    {
                        ticket = oracleRepository.ReadNextTicket(listIncomplete);
                        isNew = false;
                    }

                    if (ticket == null)
                    {

                        List<decimal> todasFilas = new List<decimal>();

                        var filas = variables.getListVariable(VariableTypeEnum.filas_classificacao);

                        todasFilas = filas.Select(x => decimal.Parse(x.variableIntegration))?.Distinct()?.ToList();

                        if (!(todasFilas?.Any() ?? false)) throw new Exception("Nenhuma fila foi configurada para essa unidade");

                        var lastTicket = ListLastTicket.FirstOrDefault(c => c.cdAtendimento.HasValue);

                        ticket = oracleRepository.ReadLastTicket(todasFilas, lastTicket);
                    }

                    if (ticket == null) return false;

                    if (ticket.cdAtendimento.HasValue)
                    {
                        SendToCelerus(oracleRepository, postgreRepository, variables, ticket, isNew);
                        return true;
                    }
                    else
                    {
                        postgreRepository.CreateRelation(new RelationEpisode
                        {
                            datetimeInclusion = DateTime.UtcNow,
                            cdTriagemAtendimento = ticket.cdTriagemAtendimento,
                            isMv = true
                        });
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

                _log.Save(LogTypeEnum.Error, (int)_appSettings.idHealthUnit,
                    $"{MethodBase.GetCurrentMethod().ReflectedType.Name}.{MethodBase.GetCurrentMethod().Name}",
                    null, null, ex);
                return true;
            }
        }

        private void SendToCelerus(IOracleMVRepository oracleRepository, IPostgreMVRepository postgreRepository, ListVariableStruct variables, TriagemAtendimento ticket, bool isNew)
        {
            var flow = variables.getVariableTolife<int>(VariableTypeEnum.filas_classificacao, ticket.cdFilaSenha.Value.ToString());

            Paciente paciente = new Paciente();
            if (ticket.cdPaciente.HasValue)
            {
                paciente = oracleRepository.GetPacienteById(ticket.cdPaciente.Value);
                if (paciente == null)
                    throw new Exception($"Id do paciente não encontrado na base ${ticket.cdPaciente}");

            }
            else
            {
                paciente.nmPaciente = ticket.nmPaciente;
                paciente.dtNascimento = ticket.dtNascimento;
                paciente.tpSexo = ticket.tpSexo.HasValue ? ticket.tpSexo.Value : 'I';
            }

            var result = OrchestratorIntegration.CreateEpisode(_appSettings.urlApiIntegration, new PostEpisodeRequest
            {
                listPriority = ticket.snPrioridadeOcto == 'S' ? new List<int> { 1 } : (ticket.snPrioridadeClassificacao == 'S' ? new List<int> { 4 } : new List<int> { }),
                episodeTicket = new EpisodeTicket
                {
                    ticketInitials = Regex.Replace(ticket.dsSenha, "[^a-zA-Z]", ""),
                    ticketSequence = int.Parse(Regex.Replace(ticket.dsSenha, "[^. 0-9]", "")),
                },
                patient = new Patient()
                {
                    patientName = paciente.nmPaciente,
                    birthDate = paciente.dtNascimento,
                    cpf = string.IsNullOrEmpty(paciente.nrCpf) ? null : Regex.Replace(paciente.nrCpf, "[^. 0-9]", ""),
                    cns = string.IsNullOrEmpty(paciente.nrCns) ? null : Regex.Replace(paciente.nrCns, "[^. 0-9]", ""),
                    motherName = paciente.nmMae,
                    idGender = paciente.tpSexo == 'M' ? 2 : (paciente.tpSexo == 'F' ? 3 : 4),
                },
                idFlow = flow,

            }, variables.getVariable<string>(VariableTypeEnum.token));

            if (result.isError)
            {
                throw new Exception(result.errorDescription);
            }
            var releation = new RelationEpisode
            {
                datetimeInclusion = DateTime.UtcNow,
                idEpisode = result.idEpisode,
                cdTriagemAtendimento = ticket.cdTriagemAtendimento,
                cdAtendimento = ticket.cdAtendimento,
                isMv = true
            };
            if (isNew)
            {
                postgreRepository.CreateRelation(releation);
            }
            else
            {
                postgreRepository.UpdateRelation(releation);
            }
        }
    }
}
