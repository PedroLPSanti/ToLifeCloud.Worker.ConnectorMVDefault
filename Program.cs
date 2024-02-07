using Microsoft.EntityFrameworkCore;
using ToLifeCloud.Worker.ConnectorMVDefault;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.OracleMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Repositories.PostgreMV;
using ToLifeCloud.Worker.ConnectorMVDefault.Worker;
using Microsoft.Extensions.Logging.Debug;
using ToLifeCloud.Worker.ConnectorMVDefault.Models.OracleMV;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHostedService, PatientTriageWorker>();
builder.Services.AddSingleton<IHostedService, TicketReaderWorker>();
builder.Services.AddSingleton<IHostedService, CallTicketWorker>();
builder.Services.AddSingleton<IHostedService, UpdateConfigWorker>();
builder.Services.AddSingleton<IHostedService, TicketEvasionWorker>();
builder.Services.AddSingleton<IHostedService, LogKeepAliveWorker>();



builder.Services.Configure<AppSettings>(appSettings =>
{

    if (!builder.Environment.IsDevelopment())
    {

        appSettings.workersDelays.ticketReader = int.Parse(Environment.GetEnvironmentVariable("ticketReaderWorkerDelay"));
        appSettings.workersDelays.patientTriage = int.Parse(Environment.GetEnvironmentVariable("patientTriageWorkerDelay"));
        appSettings.workersDelays.callTicket = int.Parse(Environment.GetEnvironmentVariable("callTicketWorkerDelay"));
        appSettings.workersDelays.updateConfig = int.Parse(Environment.GetEnvironmentVariable("updateConfigWorkerDelay"));
        appSettings.workersDelays.ticketEvasion = int.Parse(Environment.GetEnvironmentVariable("ticketEvasionWorkerDelay"));

        appSettings.ticketEvasionWorkerRun = bool.Parse(Environment.GetEnvironmentVariable("ticketEvasionWorkerRun"));
        appSettings.ticketReaderWorkerRun = bool.Parse(Environment.GetEnvironmentVariable("ticketReaderWorkerRun"));
        appSettings.patientTriageWorkerRun = bool.Parse(Environment.GetEnvironmentVariable("patientTriageWorkerRun"));
        appSettings.callTicketWorkerRun = bool.Parse(Environment.GetEnvironmentVariable("callTicketWorkerRun"));
        appSettings.updateConfigWorkerRun = bool.Parse(Environment.GetEnvironmentVariable("updateConfigWorkerRun"));
        appSettings.logKeepAliveWorkerRun = bool.Parse(Environment.GetEnvironmentVariable("logKeepAliveWorkerRun"));
        appSettings.idHealthUnit = int.Parse(Environment.GetEnvironmentVariable("idHealthUnit"));
        appSettings.log = bool.Parse(Environment.GetEnvironmentVariable("log"));
    }
    else
    {
        appSettings.log = bool.Parse(builder.Configuration[$"log"]);
        appSettings.workersDelays.ticketReader = int.Parse(builder.Configuration[$"ticketReaderWorkerDelay"]);
        appSettings.workersDelays.patientTriage = int.Parse(builder.Configuration[$"patientTriageWorkerDelay"]);
        appSettings.workersDelays.callTicket = int.Parse(builder.Configuration[$"callTicketWorkerDelay"]);
        appSettings.workersDelays.updateConfig = int.Parse(builder.Configuration[$"updateConfigWorkerDelay"]);
        appSettings.workersDelays.ticketEvasion = int.Parse(builder.Configuration[$"ticketEvasionWorkerDelay"]);

        appSettings.ticketEvasionWorkerRun = bool.Parse(builder.Configuration[$"ticketEvasionWorkerRun"]);
        appSettings.ticketReaderWorkerRun = bool.Parse(builder.Configuration[$"ticketReaderWorkerRun"]);
        appSettings.patientTriageWorkerRun = bool.Parse(builder.Configuration[$"patientTriageWorkerRun"]);
        appSettings.callTicketWorkerRun = bool.Parse(builder.Configuration[$"callTicketWorkerRun"]);
        appSettings.updateConfigWorkerRun = bool.Parse(builder.Configuration[$"updateConfigWorkerRun"]);
        appSettings.logKeepAliveWorkerRun = bool.Parse(builder.Configuration[$"logKeepAliveWorkerRun"]);
        appSettings.idHealthUnit = long.Parse(builder.Configuration[$"idHealthUnit"]);
    }

    appSettings.workersDelays.logKeepAlive = int.Parse(builder.Configuration[$"logKeepAliveWorkerDelay"]);
    appSettings.internalLoginHash = builder.Configuration[$"internalLoginHash"];
    appSettings.urlApiSrvLog = builder.Configuration[$"urlApiSrvLog"];
    appSettings.urlApiIntegration = builder.Configuration[$"urlApiIntegration"];
    appSettings.urlSRVMessageQueue = builder.Configuration[$"urlSRVMessageQueue"];
    appSettings.urlIntegrationRelationConfig = builder.Configuration[$"urlIntegrationRelationConfig"];
});

builder.Services.AddDbContext<OracleDBAMVContext>(optionsBuilder =>
{
    string connectionString = builder.Configuration[$"ConnectionStrings:MVOracleConnection"];
    string oracleVersion = builder.Configuration[$"oracleVersion"];

    if (!builder.Environment.IsDevelopment())
    {
        connectionString = $"user id={Environment.GetEnvironmentVariable("userOracle")};password={Environment.GetEnvironmentVariable("passwordOracle")};data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={Environment.GetEnvironmentVariable("HostOracle")})(PORT={Environment.GetEnvironmentVariable("PortOracle")}))(CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME={Environment.GetEnvironmentVariable("ServiceNameOracle")})))";
        oracleVersion = Environment.GetEnvironmentVariable("oracleVersion");
    }

    optionsBuilder.UseOracle(connectionString, options => options.UseOracleSQLCompatibility(oracleVersion));
});

builder.Services.AddDbContext<OracleDBASGUContext>(optionsBuilder =>
{
    string connectionString = builder.Configuration[$"ConnectionStrings:MVOracleConnection"];
    string oracleVersion = builder.Configuration[$"oracleVersion"];

    if (!builder.Environment.IsDevelopment())
    {
        connectionString = $"user id={Environment.GetEnvironmentVariable("userOracle")};password={Environment.GetEnvironmentVariable("passwordOracle")};data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={Environment.GetEnvironmentVariable("HostOracle")})(PORT={Environment.GetEnvironmentVariable("PortOracle")}))(CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME={Environment.GetEnvironmentVariable("ServiceNameOracle")})))";
        oracleVersion = Environment.GetEnvironmentVariable("oracleVersion");
    }

    optionsBuilder.UseOracle(connectionString, options => options.UseOracleSQLCompatibility(oracleVersion));
});

builder.Services.AddDbContext<PostgreMVContext>(optionsBuilder =>
{
    string connectionString = builder.Configuration[$"ConnectionStrings:MVPostgresConnection"];

    if (!builder.Environment.IsDevelopment())
    {
        connectionString = $"Host={Environment.GetEnvironmentVariable("HostPostgre")};Port=5432;Pooling=true;Database=db_integration_MV;User Id=postgres;Password=%$##tolife!);";
    }
    optionsBuilder.UseNpgsql(connectionString, config => config.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null));
});

builder.Services.AddScoped<IOracleMVRepository, OracleMVRepository>();
builder.Services.AddScoped<IPostgreMVRepository, PostgreMVRepository>();

var app = builder.Build();

//Não usar está linha de código a menos que seja necessário a criação de banco de dados de maneira autómatica 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PostgreMVContext>();
    context.Database.EnsureCreated();
}

app.Run();
