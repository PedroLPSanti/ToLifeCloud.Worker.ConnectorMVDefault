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

        public void Process()
        {
            LogBatchResponse logBatchResponse = null;
            try
            {
                using (var scope = Services.CreateScope())
                {
                    IOracleMVRepository oracleRepository = scope.ServiceProvider.GetRequiredService<IOracleMVRepository>();
                    IPostgreMVRepository postgreRepository = scope.ServiceProvider.GetRequiredService<IPostgreMVRepository>();
                    ListVariableStruct variables = postgreRepository.GetRelationConfig();

                    if (!variables.hasValues()) return;

                    string hash = variables.getVariable<string>(VariableTypeEnum.token);//Tem que ser a hash de integração;

                    var lastTicket = postgreRepository.getLastTicket();

                    List<decimal> todasFilas = new List<decimal>();

                    var filas = variables.getListVariable(VariableTypeEnum.filas_classificacao);

                    todasFilas = filas.Select(x => (decimal)x.idIntegrationVariable).ToList();

                    if (!(todasFilas?.Any() ?? false)) throw new Exception("Nenhuma fila foi configurada para essa unidade");

                    todasFilas = todasFilas.Distinct().ToList();

                    TriagemAtendimento ticket = lastTicket != null ? oracleRepository.ReadLastTicket(todasFilas, lastTicket.cdTriagemAtendimento) : oracleRepository.ReadLastTicket(todasFilas);

                    if (ticket == null) return;

                    SendToCelerus(oracleRepository, postgreRepository, variables, ticket);
                }
            }
            catch (Exception ex)
            {

                _log.Save(LogTypeEnum.Error, (int)_appSettings.idHealthUnit,
                    $"{MethodBase.GetCurrentMethod().ReflectedType.Name}.{MethodBase.GetCurrentMethod().Name}",
                    null, null, ex);
            }
        }

        private void SendToCelerus(IOracleMVRepository oracleRepository, IPostgreMVRepository postgreRepository, ListVariableStruct variables, TriagemAtendimento ticket)
        {
            var flow = variables.getVariableTolife<int>(VariableTypeEnum.filas_classificacao, ticket.cdFilaSenha.Value.ToString());

            Paciente paciente = new Paciente();
            if (ticket.cdPaciente.HasValue)
                paciente = oracleRepository.GetPacienteById(variables.getVariable<decimal>(VariableTypeEnum.multi_empresa), ticket.cdPaciente.Value);
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
                    cpf = Regex.Replace(paciente.nrCpf, "[^. 0-9]", ""),
                    cns = Regex.Replace(paciente.nrCns, "[^. 0-9]", ""),
                    motherName = paciente.nmMae,
                    idGender = paciente.tpSexo == 'M' ? 2 : (paciente.tpSexo == 'F' ? 3 : 4),
                },
                idFlow = flow,

            }, variables.getVariable<string>(VariableTypeEnum.token));

            if (result.isError)
            {
                throw new Exception(result.errorDescription);
            }

            postgreRepository.CreateRelation(new RelationEpisode
            {
                datetimeInclusion = DateTime.UtcNow,
                idEpisode = result.idEpisode,
                cdTriagemAtendimento = ticket.cdTriagemAtendimento,
                isMv = true
            });
        }
    }
}
