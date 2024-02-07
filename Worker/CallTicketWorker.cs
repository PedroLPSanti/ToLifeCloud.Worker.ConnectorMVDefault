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
    public class CallTicketWorker : BackgroundService
    {
        private readonly ILogger<CallTicketWorker> _logger;

        private readonly AppSettings _appSettings;

        private IServiceProvider Services { get; }

        public CallTicketWorker(ILogger<CallTicketWorker> logger, IOptions<AppSettings> appSettings, IServiceProvider services)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            Services = services;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            UtilLogs.PrintLog(_logger, _appSettings, "Worker callTicket running {time}", _appSettings.callTicketWorkerRun);

            if (_appSettings.callTicketWorkerRun)
            {
                
                CallTicketService callTicketService = new CallTicketService(_appSettings, Services);
                while (!stoppingToken.IsCancellationRequested)
                {
                    UtilLogs.PrintLog(_logger, _appSettings, "Worker callTicket running at: {time}", DateTimeOffset.Now);
                    callTicketService.Process();
                    await Task.Delay(_appSettings.workersDelays.callTicket, stoppingToken);
                }
            }
        });
    }
}
