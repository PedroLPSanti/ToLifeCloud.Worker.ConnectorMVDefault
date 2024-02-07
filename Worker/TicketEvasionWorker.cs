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
    public class TicketEvasionWorker : BackgroundService
    {
        private readonly ILogger<CallTicketWorker> _logger;

        private readonly AppSettings _appSettings;

        private IServiceProvider Services { get; }

        public TicketEvasionWorker(ILogger<CallTicketWorker> logger, IOptions<AppSettings> appSettings, IServiceProvider services)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            Services = services;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            UtilLogs.PrintLog(_logger, _appSettings, "Worker callTicket running {time}", _appSettings.ticketEvasionWorkerRun);

            if (_appSettings.ticketEvasionWorkerRun)
            {
                TicketEvasionService ticketEvasionService = new TicketEvasionService(_appSettings, Services);
                while (!stoppingToken.IsCancellationRequested)
                {
                    UtilLogs.PrintLog(_logger, _appSettings, "Worker callTicket running at: {time}", DateTimeOffset.Now);
                    ticketEvasionService.Process();
                    await Task.Delay(_appSettings.workersDelays.ticketEvasion, stoppingToken);
                }
            }
        });
    }
}
