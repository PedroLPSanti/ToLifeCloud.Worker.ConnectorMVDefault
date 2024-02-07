using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Worker
{
    public class PatientTriageWorker : BackgroundService
    {
        private readonly ILogger<PatientTriageWorker> _logger;

        private readonly AppSettings _appSettings;

        private IServiceProvider Services { get; }

        public PatientTriageWorker(ILogger<PatientTriageWorker> logger, IOptions<AppSettings> appSettings, IServiceProvider services)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            Services = services;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            UtilLogs.PrintLog(_logger, _appSettings, "Worker PatientTriage running {time}", _appSettings.patientTriageWorkerRun);
            if (_appSettings.patientTriageWorkerRun)
            {
                PatientTriageService patientTriageService = new PatientTriageService(_appSettings, Services);
                while (!stoppingToken.IsCancellationRequested)
                {
                    UtilLogs.PrintLog(_logger, _appSettings, "Worker PatientTriage running at: {time}", DateTime.Now);
                    patientTriageService.Process();
                    await Task.Delay(_appSettings.workersDelays.patientTriage, stoppingToken);
                }
            }
        });
    }
}
