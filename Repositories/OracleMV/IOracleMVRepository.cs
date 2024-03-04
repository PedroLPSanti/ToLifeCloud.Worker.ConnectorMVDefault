using ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Repositories.OracleMV
{
    public interface IOracleMVRepository
    {
        public decimal InsertTriage(TriagemAtendimento triagemAtendimento);
        public void UpdateTriage(TriagemAtendimento triagemAtendimento);
        public void UpdateSenhaTriagem(ListVariableStruct variables, TriageWebhookStruct triage, string cdSenha);
        public SacrClassificacaoRisco InsertClassificacaoRisco(SacrClassificacaoRisco sacrClassificacaoRisco);
        public ColetaSinalVital InsertColetaSinalVital(ColetaSinalVital coletaSinalVital);
        public void InsertColetaSinalVital(ItColetaSinalVital itColetaSinalVital);
        public Usuarios? GetUserByCpf(string cpf);
        public void InsertPaguAvaliacao(PaguAvaliacao paguAvaliacao);
        public SinalVitalStruct GetSinalVital(decimal cdSinalVital);
        public Paciente? GetPacienteByCpf(decimal cdMultiEmpresa, string? cpf = null, string? cns = null);
        public Paciente GetPacienteById(decimal cdMultiEmpresa, decimal cdPaciente);
        public string GetTriagemAtendimento(decimal cdTriagemAtendimneto);
        public void CompleteColetaSinalVital(decimal cdColetaSinalVital);
        public void InsertTempoProcesso(SacrTempoProcesso sacrTempoProcesso);
        public decimal InsertTriagemAtendimentoHist(TriagemAtendimentoHist triagemAtendimentoHist);
        public void InsertTriaAtndHisItColSinVit(decimal cdTriagemAtendimentoHist, decimal cdColetaSinalVital, decimal cdSinalVital);
        public void CallPaciente(decimal cdMultiEmpresa, decimal cdTriagemAtendimento, string nmMaquina, string tpTempoProcesso, decimal? cdAtendimento);
        public TriagemAtendimento? ReadLastTicket(List<decimal> filas, decimal? cdTriagemAtendimento = null);
        public void DeleteAtendimentoTriagem(decimal cdTriagemAtendimento);
        public void ValidateGravityConfig(decimal cdMultiEmpresa, decimal cdCorReferencia, decimal cdClassificacao);
    }
}
