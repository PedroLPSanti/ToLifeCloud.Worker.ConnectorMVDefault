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
        public string GetTriagemAtendimento(decimal cdTriagemAtendimneto);
    }
}
