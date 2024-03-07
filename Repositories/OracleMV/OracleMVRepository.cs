using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Requests;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Repositories.OracleMV
{
    public class OracleMVRepository : IOracleMVRepository
    {
        private OracleDBAMVContext _contextDBAMV;
        private OracleDBASGUContext _contextDBASGU;

        public OracleMVRepository(OracleDBAMVContext context, OracleDBASGUContext contextDBASGU)
        {
            _contextDBAMV = context;
            _contextDBASGU = contextDBASGU;
        }



        private decimal GetId(string tableName)
        {
            var command = _contextDBAMV.Database.GetDbConnection().CreateCommand();

            command.CommandType = CommandType.Text;
            command.CommandText = $"SELECT DBAMV.SEQ_{tableName}.NEXTVAL FROM DUAL";
            _contextDBAMV.Database.OpenConnection();

            try
            {
                var result = (decimal?)command.ExecuteScalar();
                return result.Value;
            }
            finally
            {
                _contextDBAMV.Database.CloseConnection();
            }
        }

        public Usuarios? GetUserByCpf(string cpf)
        {
            var query = (from usuarios in _contextDBASGU.usuarios
                         where usuarios.cpf == cpf
                         select usuarios).FirstOrDefault();
            return query;
        }

        public string GetTriagemAtendimento(decimal cdTriagemAtendimneto)
        {
            var query = (from triagemAtendimento in _contextDBAMV.triagemAtendimento
                         where triagemAtendimento.cdTriagemAtendimento == cdTriagemAtendimneto
                         select triagemAtendimento.dsSenha).FirstOrDefault();
            return query;
        }

        public TriagemAtendimento GetTriagemAtendimento()
        {
            var query = (from triagemAtendimento in _contextDBAMV.triagemAtendimento
                         select triagemAtendimento).OrderByDescending(c => c).FirstOrDefault();
            return query;
        }

        public void CallPaciente(ListVariableStruct variables, PanelCallWebhookStruct panelCall, RelationEpisode relationEpisode)
        {
            decimal cdMultiEmpresa = variables.getVariable<decimal>(VariableTypeEnum.multi_empresa);
            decimal cdTriagemAtendimento = relationEpisode.cdTriagemAtendimento;
            string nmMaquina = variables.getVariable<string>(VariableTypeEnum.nm_maquina, panelCall.idRoom);
            string tpTempoProcesso = variables.getVariable<string>(VariableTypeEnum.tp_tempo_processo);
            decimal? cdAtendimento = relationEpisode.cdAtendimento;

            var parametros = $"<cdtriagematendimento>{cdTriagemAtendimento}</cdtriagematendimento>" +
                                (cdAtendimento.HasValue ? $"<cdatendimento>{cdAtendimento}</cdatendimento>" : "") +
                                $"<cdmultiempresa>{cdMultiEmpresa}</cdmultiempresa>" +
                                $"<nmmaquina>{nmMaquina}</nmmaquina>" +
                                $"<tptempoprocesso>{tpTempoProcesso}</tptempoprocesso>";
            OracleParameter parameter = new OracleParameter("PARAMETROS", parametros);
            var sql = "CALL DBAMV.PRC_REALIZA_CHAMADA_PAINEL(:PARAMETROS)";
            int rows = _contextDBAMV.Database.ExecuteSqlRaw(sql, parameter);
        }

        public Paciente? GetPacienteByCpf(decimal cdMultiEmpresa, string? cpf = null, string? cns = null)
        {
            var query = (from paciente in _contextDBAMV.paciente
                         where paciente.cdMultiEmpresa == cdMultiEmpresa
                         select paciente).AsQueryable();

            if (!string.IsNullOrEmpty(cpf))
            {
                query = query.Where(c => c.nrCpf == cpf);
            }
            else if (!string.IsNullOrEmpty(cns))
            {
                query = query.Where(c => c.nrCns == cns);
            }

            return query.FirstOrDefault();
        }

        public Paciente GetPacienteById(decimal cdPaciente)
        {
            var query = (from paciente in _contextDBAMV.paciente
                         where paciente.cdPaciente == cdPaciente
                         select paciente).AsQueryable();
            return query.FirstOrDefault();
        }

        public decimal InsertTriage(TriagemAtendimento triagemAtendimento)
        {
            var id = GetId("TRIAGEM_ATENDIMENTO");

            triagemAtendimento.cdTriagemAtendimento = id;

            _contextDBAMV.triagemAtendimento.Add(triagemAtendimento);

            _contextDBAMV.SaveChanges();

            return triagemAtendimento.cdTriagemAtendimento;
        }

        public void UpdateTriage(TriagemAtendimento triagemAtendimento)
        {
            _contextDBAMV.triagemAtendimento.Update(triagemAtendimento);
            _contextDBAMV.SaveChanges();
        }

        public void UpdateSenhaTriagem(ListVariableStruct variables, TriageWebhookStruct triage, string dsSenha)
        {
            var cdMultiEmpresa = variables.getVariable<decimal>(VariableTypeEnum.multi_empresa);
            var cdFilaSenha = variables.getVariable<decimal>(VariableTypeEnum.fila_senha, triage.idForward.Value);
            decimal cdSenha = decimal.Parse(Regex.Match(dsSenha, @"\d+").Value);

            var query = _contextDBAMV.sacrSenhaTriagem.OrderByDescending(c => c.dhSenha)
                .Where(c => c.cdMultiEmpresa == cdMultiEmpresa &&
                            c.cdFilaSenha == cdFilaSenha &&
                            c.cdSenha == cdSenha).FirstOrDefault();

            query.cdSenha = decimal.Parse(Regex.Match(triage.ticketName, @"\d+").Value);

            _contextDBAMV.sacrSenhaTriagem.Update(query);
            _contextDBAMV.SaveChanges();
        }

        public SacrClassificacaoRisco InsertClassificacaoRisco(SacrClassificacaoRisco sacrClassificacaoRisco)
        {
            var id = GetId("SACR_CLASSIFICACAO_RISCO");

            sacrClassificacaoRisco.cdClassificacaoRisco = id;

            _contextDBAMV.sacrClassificacaoRisco.Add(sacrClassificacaoRisco);

            _contextDBAMV.SaveChanges();

            return sacrClassificacaoRisco;
        }

        public ColetaSinalVital InsertColetaSinalVital(ColetaSinalVital coletaSinalVital)
        {
            var id = GetId("COLETA_SINAL_VITAL");

            coletaSinalVital.cdColetaSinalVital = id;

            _contextDBAMV.coletaSinalVital.Add(coletaSinalVital);

            _contextDBAMV.SaveChanges();

            return coletaSinalVital;
        }

        public void CompleteColetaSinalVital(decimal cdColetaSinalVital)
        {
            var query = _contextDBAMV.coletaSinalVital.Where(c => c.cdColetaSinalVital == cdColetaSinalVital).FirstOrDefault();

            query.snFinalizado = 'S';

            _contextDBAMV.coletaSinalVital.Update(query);

            _contextDBAMV.SaveChanges();
        }

        public void InsertPaguAvaliacao(PaguAvaliacao paguAvaliacao)
        {
            var id = GetId("PAGU_AVALIACAO");

            paguAvaliacao.cdAvaliacao = id;

            _contextDBAMV.paguAvaliacao.Add(paguAvaliacao);

            _contextDBAMV.SaveChanges();
        }

        public void InsertColetaSinalVital(ItColetaSinalVital itColetaSinalVital)
        {
            _contextDBAMV.itColetaSinalVital.Add(itColetaSinalVital);
            _contextDBAMV.SaveChanges();
        }

        public SinalVitalStruct GetSinalVital(decimal cdSinalVital)
        {
            var query = (from sinalVital in _contextDBAMV.sinalVital
                         join pwSinalVitalUnidInstrAfer in _contextDBAMV.pwSinalVitalUnidInstrAfer on sinalVital.cdSinalVital equals pwSinalVitalUnidInstrAfer.cdSinalVital
                         where sinalVital.cdSinalVital == cdSinalVital
                         select new SinalVitalStruct
                         {
                             cdSinalVital = sinalVital.cdSinalVital,
                             dsSinalVital = sinalVital.dsSinalVital,
                             cdInstrumentoAfericao = pwSinalVitalUnidInstrAfer.cdInstrumentoAfericao,
                             cdUnidadeAfericao = pwSinalVitalUnidInstrAfer.cdUnidadeAfericao,
                             tpLancamento = pwSinalVitalUnidInstrAfer.tpLancamento
                         }).FirstOrDefault();
            return query;
        }

        public void ValidateGravityConfig(ListVariableStruct variables, int idGravity)
        {
            decimal cdMultiEmpresa = variables.getVariable<decimal>(VariableTypeEnum.multi_empresa);
            decimal cdCorReferencia = variables.getVariable<decimal>(VariableTypeEnum.cor_referencia, idGravity);
            decimal cdClassificacao = variables.getVariable<decimal>(VariableTypeEnum.classificacao, idGravity);

            var query = (from sacrClassificacao in _contextDBAMV.sacrClassificacao
                         join sacrProtocoloMultiEmpresa in _contextDBAMV.sacrProtocoloMultiEmpresa
                            on sacrClassificacao.cdProtocolo equals sacrProtocoloMultiEmpresa.cdProtocolo
                         join sacrProtocolo in _contextDBAMV.sacrProtocolo
               on sacrClassificacao.cdProtocolo equals sacrProtocolo.cdProtocolo
                         join sacrCorReferencia in _contextDBAMV.sacrCorReferencia
               on sacrClassificacao.cdCorReferencia equals sacrCorReferencia.cdCorReferencia
                         where sacrProtocoloMultiEmpresa.cdMultiEmpresa == cdMultiEmpresa
                         && sacrCorReferencia.cdCorReferencia == cdCorReferencia
                         && sacrClassificacao.cdClassificacao == cdClassificacao
                         select sacrClassificacao)
               .FirstOrDefault();
            if (query == null)
            {
                throw new Exception($"Unidade não está configurada para usar os valores de cor referência: {cdCorReferencia} e classificação: {cdClassificacao}");
            }
        }


        public void InsertTempoProcesso(SacrTempoProcesso sacrTempoProcesso)
        {
            var id = GetId("SACR_TEMPO_PROCESSO");

            sacrTempoProcesso.cdTempoProcesso = id;

            _contextDBAMV.sacrTempoProcesso.Add(sacrTempoProcesso);

            _contextDBAMV.SaveChanges();
        }

        public decimal InsertTriagemAtendimentoHist(TriagemAtendimentoHist triagemAtendimentoHist)
        {
            var id = GetId("TRIAGEM_ATENDIMENTO_HIST");

            triagemAtendimentoHist.cdTriagemAtendimentoHist = id;

            _contextDBAMV.triagemAtendimentoHist.Add(triagemAtendimentoHist);

            _contextDBAMV.SaveChanges();

            return id;
        }

        public void InsertTriaAtndHisItColSinVit(decimal cdTriagemAtendimentoHist, decimal cdColetaSinalVital, decimal cdSinalVital)
        {
            _contextDBAMV.triaAtndHisItColSinVit.Add(new TriaAtndHisItColSinVit
            {
                cdTriagemAtendimentoHist = cdTriagemAtendimentoHist,
                cdColetaSinalVital = cdColetaSinalVital,
                cdSinalVital = cdSinalVital
            });

            _contextDBAMV.SaveChanges();
        }

        public TriagemAtendimento? ReadLastTicket(List<decimal> filas, RelationEpisode? episode)
        {
            var query = _contextDBAMV.triagemAtendimento.Where(c =>
                c.cdFilaSenha.HasValue && filas.Contains(c.cdFilaSenha.Value)
            ).AsQueryable();

            if (episode != null)
                query = query.Where(c => c.cdTriagemAtendimento > episode.cdTriagemAtendimento);

            return query.OrderBy(c => c.cdTriagemAtendimento).FirstOrDefault();
        }

        public TriagemAtendimento? ReadNextTicket(List<decimal> listTriagemAtendimento)
        {
            return _contextDBAMV.triagemAtendimento.Where(c =>
                listTriagemAtendimento.Contains(c.cdTriagemAtendimento) && c.cdAtendimento.HasValue
            ).OrderBy(c => c.cdTriagemAtendimento).FirstOrDefault();
        }

        public void DeleteAtendimentoTriagem(decimal cdTriagemAtendimento)
        {
            var query = _contextDBAMV.triagemAtendimento.FirstOrDefault(c => c.cdTriagemAtendimento == cdTriagemAtendimento);
            query.dhRemovido = DateTime.Now;
            query.dsObservacaoRemovido = "EVASÃO";
            _contextDBAMV.triagemAtendimento.Update(query);
            _contextDBAMV.SaveChanges();
        }
    }
}
