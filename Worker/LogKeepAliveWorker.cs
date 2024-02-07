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
    public class LogKeepAliveWorker : BackgroundService
    {
        private readonly ILogger<TicketReaderWorker> _logger;
        private IServiceProvider Services { get; }
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _enviornment;

        public LogKeepAliveWorker(ILogger<TicketReaderWorker> logger, IOptions<AppSettings> appSettings, IWebHostEnvironment environment, IServiceProvider services)
        {
            Services = services;
            _logger = logger;
            _appSettings = appSettings.Value;
            _enviornment = environment;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            UtilLogs.PrintLog(_logger, _appSettings, "Worker LogKeepAliveService running {time}", _appSettings.logKeepAliveWorkerRun);
            if (_appSettings.logKeepAliveWorkerRun)
            {
                LogKeepAliveService logKeepAlive = new LogKeepAliveService(_appSettings, Services);
                while (!stoppingToken.IsCancellationRequested)
                {
                    UtilLogs.PrintLog(_logger, _appSettings, "Worker LogKeepAliveService running at: {time}", DateTime.Now);
                    logKeepAlive.Process();
                    await Task.Delay(_appSettings.workersDelays.logKeepAlive, stoppingToken);
                }
            }
        });
    }
}
