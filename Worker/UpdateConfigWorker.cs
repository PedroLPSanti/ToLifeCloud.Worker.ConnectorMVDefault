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
    public class UpdateConfigWorker : BackgroundService
    {
        private readonly ILogger<UpdateConfigWorker> _logger;
        private IServiceProvider Services { get; }
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _enviornment;

        public UpdateConfigWorker(ILogger<UpdateConfigWorker> logger, IOptions<AppSettings> appSettings, IWebHostEnvironment environment, IServiceProvider services)
        {
            Services = services;
            _logger = logger;
            _appSettings = appSettings.Value;
            _enviornment = environment;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            UtilLogs.PrintLog(_logger, _appSettings, "Worker UpdateConfig running {time}", _appSettings.updateConfigWorkerRun);
            if (_appSettings.updateConfigWorkerRun)
            {
                UpdateConfigService updateConfig = new UpdateConfigService(_appSettings, Services);
                while (!stoppingToken.IsCancellationRequested)
                {
                    UtilLogs.PrintLog(_logger, _appSettings, "Worker UpdateConfig running at: {time}", DateTimeOffset.Now);
                    updateConfig.Process();
                    await Task.Delay(_appSettings.workersDelays.updateConfig, stoppingToken);
                }
            }
        });
    }
}
